package com.siarheikuchuk.butil.core.ftps

import java.io.OutputStreamWriter
import java.net.Inet6Address
import java.net.InetAddress
import java.net.ServerSocket
import java.net.Socket
import java.net.SocketTimeoutException
import java.nio.charset.Charset
import java.nio.charset.StandardCharsets
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Date
import java.util.Locale
import java.util.TimeZone
import javax.net.ssl.SSLContext
import javax.net.ssl.SSLSocket
import javax.net.ssl.SSLSocketFactory

class FtpsClientSession(
    log: FtpsLog,
    private var socket: Socket,
    private val users: List<FtpsUserAccount>,
    private val sslContext: SSLContext?,
    private val fileSystem: FileSystemProvider,
    private val loginThrottle: LoginThrottle,
) {
    private val log: FtpsLog = SanitizingLog(log)
    private var writer: OutputStreamWriter = OutputStreamWriter(socket.getOutputStream(), LATIN1)
    private var pushedByte = -1
    private val controlByte = ByteArray(1)
    private var encoding: Charset = LATIN1
    private var username: String? = null
    private var user: FtpsUserAccount? = null
    private var authenticated = false
    private var path = VirtualPath()
    private var renameFrom: VirtualPath? = null
    private val ioLock = Any()
    @Volatile private var stopping = false
    private var dataListener: ServerSocket? = null
    private var dataSocket: Socket? = null
    private var passive = false
    private var extendedPassiveOnly = false
    private var dataProtection = DataProtection.Clear
    private val clientAddress = socket.remoteSocketAddress?.toString() ?: "unknown"
    private val clientIpKey = socket.inetAddress?.let { LoginThrottle.key(it) }
    private var disconnect = false
    private val mlsSelected = mutableSetOf("type", "size", "modify", "perm")
    @Volatile private var transferProgressMs = 0L
    @Volatile private var transferActive = false
    private val connectedMs = System.currentTimeMillis()

    fun close() {
        val pasv: ServerSocket?
        val data: Socket?
        synchronized(ioLock) {
            stopping = true
            pasv = dataListener
            data = dataSocket
        }
        try { socket.close() } catch (_: Exception) {}
        try { pasv?.close() } catch (_: Exception) {}
        try { data?.close() } catch (_: Exception) {}
    }

    fun handle() {
        try {
            send(220, "FTPS Server Ready")
            while (!stopping) {
                val line = readCommandLine() ?: break
                val logLine = if (line.startsWith("PASS ", ignoreCase = true)) "PASS ****" else line
                log.debug("[$clientAddress] >> $logLine")
                val space = line.indexOf(' ')
                val command = (if (space < 0) line else line.substring(0, space)).uppercase(Locale.ROOT)
                val argument = if (space < 0) "" else line.substring(space + 1)
                process(command, argument)
                if (command == "QUIT" || disconnect) break
            }
        } catch (e: Exception) {
            sessionError("[$clientAddress] Session error", e)
        } finally {
            try { socket.close() } catch (_: Exception) {}
            try { dataListener?.close() } catch (_: Exception) {}
        }
    }

    private fun process(command: String, argument: String) {
        try {
            when (command) {
                "USER" -> handleUser(argument)
                "PASS" -> handlePass(argument)
                "AUTH" -> handleAuth(argument)
                "PBSZ" -> handlePbsz()
                "PROT" -> handleProt(argument)
                "PWD", "XPWD" -> handlePwd()
                "CWD", "XCWD" -> handleCwd(argument)
                "CDUP", "XCUP" -> handleCdup()
                "MKD", "XMKD" -> handleMkd(argument)
                "RMD", "XRMD" -> handleRmd(argument)
                "DELE" -> handleDele(argument)
                "RNFR" -> handleRnfr(argument)
                "RNTO" -> handleRnto(argument)
                "TYPE" -> {
                    if (!checkAuth()) send(530, "Not logged in")
                    else send(200, "Type set to ${argument.uppercase(Locale.ROOT)}")
                }
                "PASV" -> handlePasv()
                "EPSV" -> handleEpsv(argument)
                "LIST" -> handleList(argument)
                "MLSD" -> handleMlsd(argument)
                "MLST" -> handleMlst(argument)
                "NLST" -> handleNlst(argument)
                "RETR" -> handleRetr(argument)
                "STOR" -> handleStor(argument)
                "SIZE" -> handleSize(argument)
                "MDTM" -> handleMdtm(argument)
                "SYST" -> send(215, "UNIX Type: L8")
                "FEAT" -> handleFeat()
                "OPTS" -> handleOpts(argument)
                "NOOP" -> send(200, "OK")
                "QUIT" -> {
                    send(221, "Goodbye")
                    log.info("[$clientAddress] User ${username ?: "anonymous"} logged out")
                }
                else -> send(502, "Command '$command' not implemented")
            }
        } catch (e: SecurityException) {
            log.warn("[$clientAddress] Access denied: ${e.message}")
            send(550, "Permission denied")
        } catch (e: Exception) {
            sessionError("[$clientAddress] Command error: $command", e)
            send(550, "Error")
        }
    }

    private fun handleOpts(argument: String) {
        val args = argument.split(Regex("\\s+")).filter { it.isNotEmpty() }
        if (args.isNotEmpty() && args[0].equals("MLST", true)) {
            mlsSelected.clear()
            if (args.size >= 2) {
                val names = args.drop(1).joinToString(" ").split(';').map { it.trim() }.filter { it.isNotEmpty() }
                for (name in names) {
                    val match = MLS_FACTS.firstOrNull { it.equals(name, true) }
                    if (match != null) mlsSelected += match
                }
            }
            var selected = MLS_FACTS.filter { it in mlsSelected }.joinToString(";")
            if (selected.isNotEmpty()) selected += ";"
            send(200, "MLST OPTS $selected")
            return
        }
        if (args.isNotEmpty() && args[0].equals("UTF8", true)) {
            if (args.size == 1 || args[1].equals("ON", true)) {
                encoding = StandardCharsets.UTF_8
                recreateStreams(clearPushback = false)
                send(200, "UTF8 mode enabled")
                return
            }
            if (args[1].equals("OFF", true)) {
                encoding = LATIN1
                recreateStreams(clearPushback = false)
                send(200, "UTF8 mode disabled")
                return
            }
        }
        send(501, "Invalid OPTS command")
    }

    private fun recreateStreams(clearPushback: Boolean) {
        if (clearPushback) pushedByte = -1
        writer.flush()
        writer = OutputStreamWriter(socket.getOutputStream(), encoding)
    }

    private fun readCommandLine(): String? {
        val now = System.currentTimeMillis()
        val timeoutMs = if (authenticated) AUTHENTICATED_IDLE_MS else unauthenticatedReadTimeoutMs(now - connectedMs)
        if (timeoutMs <= 0) {
            log.warn("[$clientAddress] Closed control connection: not logged in within ${LOGIN_TIMEOUT_MS / 1000} seconds")
            return null
        }
        val deadline = now + timeoutMs
        val raw = java.io.ByteArrayOutputStream()
        try {
            while (true) {
                val next = readControlByte(deadline)
                if (next < 0) return if (raw.size() == 0) null else String(raw.toByteArray(), encoding)
                if (next == '\n'.code) return String(raw.toByteArray(), encoding)
                if (next == '\r'.code) {
                    val following = readControlByte(deadline)
                    if (following >= 0 && following != '\n'.code) pushedByte = following
                    return String(raw.toByteArray(), encoding)
                }
                if (raw.size() >= MAX_COMMAND_LINE_LENGTH) {
                    log.warn("[$clientAddress] Closed control connection: command line exceeded $MAX_COMMAND_LINE_LENGTH characters")
                    return null
                }
                raw.write(next)
            }
        } catch (_: SocketTimeoutException) {
            log.warn("[$clientAddress] Closed control connection: idle for ${timeoutMs / 1000} seconds")
            return null
        }
    }

    private fun readControlByte(deadlineMs: Long): Int {
        if (pushedByte >= 0) {
            val pushed = pushedByte
            pushedByte = -1
            return pushed
        }
        val remaining = deadlineMs - System.currentTimeMillis()
        if (remaining <= 0) throw SocketTimeoutException()
        socket.soTimeout = remaining.coerceAtMost(Int.MAX_VALUE.toLong()).toInt().coerceAtLeast(1)
        val read = socket.getInputStream().read(controlByte)
        if (read <= 0) return -1
        return controlByte[0].toInt() and 0xFF
    }

    private fun handleUser(name: String) {
        if (!requireControlTls(530)) return
        username = name
        log.info("[$clientAddress] User login attempt: $name")
        send(331, "Password required")
    }

    private fun handlePass(password: String) {
        if (!requireControlTls(530)) return
        val name = username
        if (name.isNullOrEmpty()) {
            send(503, "Login with USER first")
            return
        }
        // A connection opened before the lockout must not keep guessing through it.
        if (clientIpKey != null && loginThrottle.isLocked(clientIpKey, System.currentTimeMillis())) {
            log.warn("[$clientAddress] Rejected login for user $name: too many failed logins")
            username = null
            send(530, "Login incorrect")
            disconnect = true
            return
        }
        val found = users.firstOrNull { it.login == name }
        if (found != null && found.password == password) {
            user = found
            authenticated = true
            path = VirtualPath()
            clientIpKey?.let { loginThrottle.registerSuccess(it) }
            log.info("[$clientAddress] User logged in: $name")
            send(230, "User logged in")
        } else {
            try {
                Thread.sleep(LoginThrottle.FAILURE_DELAY_MS)
            } catch (_: InterruptedException) {
                Thread.currentThread().interrupt()
            }
            val locked = clientIpKey?.let { loginThrottle.registerFailure(it, System.currentTimeMillis()) } == true
            log.warn("[$clientAddress] Failed login attempt for user: $name")
            username = null
            send(530, "Login incorrect")
            if (locked) disconnect = true
        }
    }

    private fun handleAuth(argument: String) {
        val ctx = sslContext
        if (ctx == null) {
            send(502, "TLS not available")
            return
        }
        if (!argument.equals("TLS", true) && !argument.equals("SSL", true)) {
            send(504, "AUTH type not supported")
            return
        }
        send(234, "AUTH command ok. Expecting TLS Negotiation.")
        try {
            val factory = ctx.socketFactory as SSLSocketFactory
            val ssl = factory.createSocket(socket, socket.inetAddress.hostAddress, socket.port, true) as SSLSocket
            ssl.useClientMode = false
            restrictTls(ssl)
            ssl.startHandshake()
            socket = ssl
            recreateStreams(clearPushback = true)
            log.info("[$clientAddress] TLS enabled on control connection")
        } catch (e: Exception) {
            sessionError("[$clientAddress] TLS negotiation failed", e)
            disconnect = true
        }
    }

    private fun handlePbsz() {
        if (!requireControlTls(534)) return
        send(200, "PBSZ=0")
    }

    private fun handleProt(argument: String) {
        if (!requireControlTls(534)) return
        when (argument.uppercase(Locale.ROOT)) {
            "P" -> {
                dataProtection = DataProtection.Protected
                send(200, "Protection level set to Private")
            }
            "C" -> {
                dataProtection = DataProtection.Clear
                send(200, "Protection level set to Clear")
            }
            else -> send(504, "PROT type not supported")
        }
    }

    private fun handlePwd() {
        if (!checkAuth()) {
            send(530, "Not logged in")
            return
        }
        val current = path.toFtpsPath()
        info("get current dir", current)
        send(257, "\"$current\" is current directory")
    }

    private fun handleCwd(directory: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val result = path.append(directory)
        info("change directory", result.toFtpsPath())
        try {
            if (fileSystem.directoryExists(user!!.folder, result.parts)) {
                path = result
                send(250, "Directory changed")
            } else {
                send(550, "Directory not found")
            }
        } catch (e: SecurityException) {
            sessionError("[$clientAddress] Attempt to change directory to: $directory", e)
            send(550, "Directory not found")
        }
    }

    private fun handleCdup() {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        path = path.goUp()
        info("change directory up", path.toFtpsPath())
        send(250, "Directory changed")
    }

    private fun handleMkd(directory: String) {
        if (!checkAuth() || !checkPerm(read = false, write = true)) {
            send(550, "Permission denied")
            return
        }
        val p = path.append(directory)
        info("create directory", p.toFtpsPath())
        try {
            fileSystem.createDirectory(user!!.folder, p.parts)
            send(257, "\"${p.toFtpsPath()}\" created")
        } catch (e: Exception) {
            sessionError("Failed to create directory: ${p.toFtpsPath()}", e)
            send(550, "Cannot create directory")
        }
    }

    private fun handleRmd(directory: String) {
        if (!checkAuth() || !checkPerm(read = false, write = true)) {
            send(550, "Permission denied")
            return
        }
        val p = path.append(directory)
        info("delete directory", p.toFtpsPath())
        try {
            if (fileSystem.directoryExists(user!!.folder, p.parts)) {
                fileSystem.directoryDelete(user!!.folder, p.parts)
                send(250, "Directory removed")
            } else {
                send(550, "Directory not found")
            }
        } catch (e: Exception) {
            sessionError("Failed to delete directory: ${p.toFtpsPath()}", e)
            send(550, "Cannot remove directory")
        }
    }

    private fun handleDele(filename: String) {
        if (!checkAuth() || !checkPerm(read = false, write = true)) {
            send(550, "Permission denied")
            return
        }
        val p = path.append(filename)
        info("delete file", p.toFtpsPath())
        try {
            if (fileSystem.fileExists(user!!.folder, p.parts)) {
                fileSystem.fileDelete(user!!.folder, p.parts)
                send(250, "File deleted")
            } else {
                send(550, "File not found")
            }
        } catch (e: Exception) {
            sessionError("Failed to delete file: ${p.toFtpsPath()}", e)
            send(550, "Cannot delete file")
        }
    }

    private fun handleRnfr(filename: String) {
        if (!checkAuth() || !checkPerm(read = false, write = true)) {
            send(550, "Permission denied")
            return
        }
        val p = path.append(filename)
        info("rename from", p.toFtpsPath())
        renameFrom = p
        if (fileSystem.fileExists(user!!.folder, p.parts) || fileSystem.directoryExists(user!!.folder, p.parts)) {
            send(350, "Ready for RNTO")
        } else {
            renameFrom = null
            send(550, "File/directory not found")
        }
    }

    private fun handleRnto(filename: String) {
        if (!checkAuth() || !checkPerm(read = false, write = true)) {
            send(550, "Permission denied")
            return
        }
        val from = renameFrom
        if (from == null) {
            send(503, "RNFR required first")
            return
        }
        val p = path.append(filename)
        info("rename to", p.toFtpsPath())
        try {
            when {
                fileSystem.fileExists(user!!.folder, from.parts) -> {
                    fileSystem.fileMove(user!!.folder, from.parts, p.parts)
                    send(250, "File renamed")
                }
                fileSystem.directoryExists(user!!.folder, from.parts) -> {
                    fileSystem.directoryMove(user!!.folder, from.parts, p.parts)
                    send(250, "Directory renamed")
                }
                else -> send(550, "Rename failed")
            }
        } catch (e: Exception) {
            sessionError("rename failed", e)
            send(550, "Rename failed")
        } finally {
            renameFrom = null
        }
    }

    private fun handlePasv() {
        if (!checkAuth()) {
            send(530, "Not logged in")
            return
        }
        if (extendedPassiveOnly) {
            send(501, "EPSV ALL in effect")
            return
        }
        if (canonicalHost(socket.localAddress).size != 4) {
            send(522, "Network protocol not supported, use (2)")
            return
        }
        enterPassive(extended = false)
    }

    // EPSV carries only the port. The client connects to the same address it used for the
    // control connection, which is how an IPv6 transfer works. PASV cannot name an IPv6 address.
    private fun handleEpsv(argument: String) {
        if (!checkAuth()) {
            send(530, "Not logged in")
            return
        }
        val requested = argument.trim()
        if (requested.equals("ALL", ignoreCase = true)) {
            extendedPassiveOnly = true
            send(200, "EPSV ALL ok")
            return
        }
        if (requested.isNotEmpty() && requested != "1" && requested != "2") {
            send(501, "Invalid EPSV argument")
            return
        }
        val family = if (canonicalHost(socket.localAddress).size == 4) "1" else "2"
        if (requested.isNotEmpty() && requested != family) {
            send(522, "Network protocol not supported, use ($family)")
            return
        }
        enterPassive(extended = true)
    }

    private fun enterPassive(extended: Boolean) {
        closePasv()
        val local = bindAddress(socket.localAddress)
        val listener = ServerSocket(0, 1, local)
        listener.soTimeout = DATA_CONNECT_TIMEOUT_MS.toInt()
        synchronized(ioLock) {
            if (stopping) {
                try { listener.close() } catch (_: Exception) {}
                return
            }
            dataListener = listener
        }
        passive = true
        val port = listener.localPort
        val response = if (extended) {
            "Entering Extended Passive Mode (|||$port|)"
        } else {
            val ip = canonicalHost(local)
            "Entering Passive Mode (${ip[0].toUByte()},${ip[1].toUByte()},${ip[2].toUByte()},${ip[3].toUByte()},${port / 256},${port % 256})"
        }
        log.debug("[$clientAddress] $response")
        send(if (extended) 229 else 227, response)
    }

    private fun wrapData(client: Socket): Socket {
        if (dataProtection != DataProtection.Protected || sslContext == null) return client
        val factory = sslContext.socketFactory as SSLSocketFactory
        val ssl = factory.createSocket(client, client.inetAddress.hostAddress, client.port, true) as SSLSocket
        ssl.useClientMode = false
        ssl.soTimeout = DATA_IDLE_TIMEOUT_MS.toInt()
        restrictTls(ssl)
        ssl.startHandshake()
        val control = socket as? SSLSocket
        if (control == null || !sameTlsSession(control, ssl)) {
            log.warn("[$clientAddress] Rejected data connection: TLS session was not resumed from the control connection")
            try { ssl.close() } catch (_: Exception) {}
            throw javax.net.ssl.SSLHandshakeException("Data connection did not resume the control TLS session")
        }
        synchronized(ioLock) {
            if (stopping) {
                try { ssl.close() } catch (_: Exception) {}
                throw java.net.SocketException("Server stopped")
            }
            dataSocket = ssl
        }
        return ssl
    }

    private fun acceptData(): Socket? {
        val listener = dataListener
        if (!passive || listener == null) {
            send(425, "Use PASV or EPSV first")
            return null
        }
        if (!dataEncryptedOk()) {
            send(534, "Data connection must be encrypted; use AUTH TLS and PROT P")
            return null
        }
        return try {
            val accepted = listener.accept()
            // RFC 2577: the data peer must be the same host as the control connection.
            if (!samePeer(socket.inetAddress, accepted.inetAddress)) {
                log.warn("[$clientAddress] Rejected data connection from unexpected address ${accepted.remoteSocketAddress}")
                try { accepted.close() } catch (_: Exception) {}
                send(425, "Can't open data connection")
                null
            } else {
                retainData(accepted)
            }
        } catch (_: SocketTimeoutException) {
            log.warn("[$clientAddress] No data connection arrived within ${DATA_CONNECT_TIMEOUT_MS / 1000} seconds")
            send(425, "Can't open data connection")
            null
        } catch (_: Exception) {
            send(425, "Can't open data connection")
            null
        }
    }

    // A transfer that moves no bytes must not hold the session slot forever. The watchdog
    // closes the data socket after DATA_IDLE_TIMEOUT_MS without progress; closing unblocks
    // a read or write stuck in the transfer loop. Progress restarts the budget.
    private fun beginTransferWatchdog(socket: Socket) {
        transferProgressMs = System.currentTimeMillis()
        transferActive = true
        val watchdog = Thread {
            while (transferActive && !stopping) {
                try {
                    Thread.sleep(5_000)
                } catch (_: InterruptedException) {
                    return@Thread
                }
                if (transferActive && System.currentTimeMillis() - transferProgressMs > DATA_IDLE_TIMEOUT_MS) {
                    log.warn("[$clientAddress] Closed data connection: no bytes moved for ${DATA_IDLE_TIMEOUT_MS / 1000} seconds")
                    try {
                        socket.close()
                    } catch (_: Exception) {
                    }
                    return@Thread
                }
            }
        }
        watchdog.isDaemon = true
        watchdog.start()
    }

    private fun endTransferWatchdog() {
        transferActive = false
    }

    private fun copyWithProgress(input: java.io.InputStream, output: java.io.OutputStream) {
        val buffer = ByteArray(64 * 1024)
        while (true) {
            val n = input.read(buffer)
            if (n < 0) return
            output.write(buffer, 0, n)
            transferProgressMs = System.currentTimeMillis()
        }
    }

    private fun closePasv() {
        val pasv: ServerSocket?
        synchronized(ioLock) {
            pasv = dataListener
            dataListener = null
            dataSocket = null
        }
        try { pasv?.close() } catch (_: Exception) {}
        passive = false
    }

    private fun retainData(accepted: Socket): Socket? {
        synchronized(ioLock) {
            if (stopping) {
                try { accepted.close() } catch (_: Exception) {}
                return null
            }
            dataSocket = accepted
            return accepted
        }
    }

    private fun handleList(directory: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val dataClient = acceptData() ?: return
        val p = path.append(directory.ifBlank { "." })
        info("get directory contents", p.toFtpsPath())
        send(150, "Opening data connection")
        try {
            wrapData(dataClient).use { sock ->
                OutputStreamWriter(sock.getOutputStream(), StandardCharsets.UTF_8).use { out ->
                    if (fileSystem.directoryExists(user!!.folder, p.parts)) {
                        beginTransferWatchdog(sock)
                        try {
                            for (entry in fileSystem.directoryEntries(user!!.folder, p.parts)) {
                                val permissions = if (entry.isDirectory) "drwxr-xr-x" else "-rw-r--r--"
                                val size = if (entry.isDirectory) "0" else entry.length.toString()
                                val modified = formatUnixListDate(entry.lastWriteTimeMillis)
                                out.write("$permissions 1 owner group ${size.padStart(15)} $modified ${entry.fileName}\r\n")
                                transferProgressMs = System.currentTimeMillis()
                            }
                            out.flush()
                        } finally {
                            endTransferWatchdog()
                        }
                    }
                }
            }
            send(226, "Transfer complete")
        } catch (e: Exception) {
            sessionError("List failed", e)
            send(550, "List failed")
        } finally {
            closePasv()
        }
    }

    private fun handleMlsd(directory: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val dataClient = acceptData() ?: return
        val p = path.append(directory.ifBlank { "." })
        info("mlsd", p.toFtpsPath())
        try {
            if (!fileSystem.directoryExists(user!!.folder, p.parts)) {
                dataClient.close()
                send(550, "Directory not found")
                return
            }
            send(150, "Opening data connection")
            wrapData(dataClient).use { sock ->
                OutputStreamWriter(sock.getOutputStream(), StandardCharsets.UTF_8).use { out ->
                    beginTransferWatchdog(sock)
                    try {
                        for (entry in fileSystem.directoryEntries(user!!.folder, p.parts)) {
                            out.write(formatMlsRecord(mlsType(entry.fileName, entry.isDirectory), entry.lastWriteTimeMillis, entry.length, entry.isDirectory, entry.fileName) + "\r\n")
                            transferProgressMs = System.currentTimeMillis()
                        }
                        out.flush()
                    } finally {
                        endTransferWatchdog()
                    }
                }
            }
            send(226, "Transfer complete")
        } catch (e: Exception) {
            sessionError("MLSD failed", e)
            send(550, "MLSD failed")
        } finally {
            closePasv()
        }
    }

    private fun handleMlst(pathname: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val p = if (pathname.isBlank()) path else path.append(pathname)
        val ftpsPath = p.toFtpsPath()
        info("mlst", ftpsPath)
        when {
            fileSystem.directoryExists(user!!.folder, p.parts) -> {
                val self = fileSystem.directoryEntries(user!!.folder, p.parts).firstOrNull { it.fileName == "." }
                val isCurrent = pathname.isBlank() || pathname == "." || pathname == "./"
                val type = if (isCurrent) "cdir" else "dir"
                val display = if (isCurrent) ftpsPath else pathname
                sendLine("250-Listing $ftpsPath")
                sendLine(" " + formatMlsRecord(type, self?.lastWriteTimeMillis ?: System.currentTimeMillis(), 0, true, display))
                send(250, "End")
            }
            fileSystem.fileExists(user!!.folder, p.parts) -> {
                val display = if (pathname.isBlank()) ftpsPath else pathname
                sendLine("250-Listing $ftpsPath")
                sendLine(
                    " " + formatMlsRecord(
                        "file",
                        fileSystem.getFileLastWriteTimeUtcMillis(user!!.folder, p.parts),
                        fileSystem.getFileLength(user!!.folder, p.parts),
                        false,
                        display,
                    )
                )
                send(250, "End")
            }
            else -> send(550, "File not found")
        }
    }

    private fun handleNlst(directory: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val dataClient = acceptData() ?: return
        val p = path.append(directory.ifBlank { "." })
        info("get directory contents 2", p.toFtpsPath())
        send(150, "Opening data connection")
        try {
            wrapData(dataClient).use { sock ->
                OutputStreamWriter(sock.getOutputStream(), StandardCharsets.UTF_8).use { out ->
                    if (fileSystem.directoryExists(user!!.folder, p.parts)) {
                        beginTransferWatchdog(sock)
                        try {
                            for (entry in fileSystem.directoryEntries(user!!.folder, p.parts)) {
                                out.write(entry.fileName + "\r\n")
                                transferProgressMs = System.currentTimeMillis()
                            }
                            out.flush()
                        } finally {
                            endTransferWatchdog()
                        }
                    }
                }
            }
            send(226, "Transfer complete")
        } catch (e: Exception) {
            sessionError("List failed", e)
            send(550, "List failed")
        } finally {
            closePasv()
        }
    }

    private fun handleRetr(filename: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val dataClient = acceptData() ?: return
        val p = path.append(filename)
        info("download", p.toFtpsPath())
        if (!fileSystem.fileExists(user!!.folder, p.parts)) {
            dataClient.close()
            send(550, "File not found")
            return
        }
        send(150, "Opening data connection")
        try {
            wrapData(dataClient).use { sock ->
                beginTransferWatchdog(sock)
                try {
                    fileSystem.fileOpenRead(user!!.folder, p.parts).use { input ->
                        copyWithProgress(input, sock.getOutputStream())
                        sock.getOutputStream().flush()
                    }
                } finally {
                    endTransferWatchdog()
                }
            }
            log.info("[$clientAddress] Download complete: $p")
            send(226, "Transfer complete")
        } catch (e: Exception) {
            sessionError("Download failed: ${p.toFtpsPath()}", e)
            send(550, "Transfer failed")
        } finally {
            closePasv()
        }
    }

    private fun handleStor(filename: String) {
        if (!checkAuth() || !checkPerm(read = false, write = true)) {
            send(550, "Permission denied")
            return
        }
        val dataClient = acceptData() ?: return
        val p = path.append(filename)
        info("upload", p.toFtpsPath())
        send(150, "Opening data connection for ${p.toFtpsPath()}")
        try {
            wrapData(dataClient).use { sock ->
                beginTransferWatchdog(sock)
                try {
                    fileSystem.fileCreate(user!!.folder, p.parts).use { output ->
                        copyWithProgress(sock.getInputStream(), output)
                        output.flush()
                    }
                } finally {
                    endTransferWatchdog()
                }
            }
            send(226, "Transfer complete")
        } catch (e: Exception) {
            sessionError("Upload failed: ${p.toFtpsPath()}", e)
            send(550, "Transfer failed")
        } finally {
            closePasv()
        }
    }

    private fun handleSize(filename: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val p = path.append(filename)
        info("get size", p.toFtpsPath())
        if (fileSystem.fileExists(user!!.folder, p.parts)) {
            send(213, fileSystem.getFileLength(user!!.folder, p.parts).toString())
        } else {
            send(550, "File not found")
        }
    }

    private fun handleMdtm(filename: String) {
        if (!checkAuth() || !checkPerm(read = true, write = false)) {
            send(550, "Permission denied")
            return
        }
        val p = path.append(filename)
        info("get modified time", p.toFtpsPath())
        if (fileSystem.fileExists(user!!.folder, p.parts)) {
            val utc = fileSystem.getFileLastWriteTimeUtcMillis(user!!.folder, p.parts)
            val fmt = SimpleDateFormat("yyyyMMddHHmmss", Locale.US).apply { timeZone = TimeZone.getTimeZone("UTC") }
            send(213, fmt.format(Date(utc)))
        } else {
            send(550, "File not found")
        }
    }

    private fun handleFeat() {
        val mlst = MLS_FACTS.joinToString(";") { if (it in mlsSelected) "$it*" else it } + ";"
        listOf(
            "211-Features:",
            " AUTH TLS",
            " PBSZ",
            " PROT",
            " EPSV",
            " SIZE",
            " MDTM",
            " MLST $mlst",
            " UTF8",
            "211 End",
        ).forEach { sendLine(it) }
    }

    private fun formatMlsRecord(type: String, lastWriteMillis: Long, size: Long, isDirectory: Boolean, pathname: String): String {
        val facts = mutableListOf<String>()
        if ("type" in mlsSelected) facts += "type=$type"
        if ("size" in mlsSelected && !isDirectory) facts += "size=$size"
        if ("modify" in mlsSelected) {
            val fmt = SimpleDateFormat("yyyyMMddHHmmss", Locale.US).apply { timeZone = TimeZone.getTimeZone("UTC") }
            facts += "modify=${fmt.format(Date(lastWriteMillis))}"
        }
        if ("perm" in mlsSelected) facts += "perm=${mlsPerm(isDirectory)}"
        val factString = if (facts.isEmpty()) "" else facts.joinToString(";") + ";"
        return "$factString $pathname"
    }

    private fun mlsPerm(isDirectory: Boolean): String {
        val read = user?.canRead == true
        val write = user?.canWrite == true
        return if (isDirectory) {
            buildString {
                if (write) append("c")
                if (write) append("d")
                if (read) append("e")
                if (write) append("f")
                if (read) append("l")
                if (write) append("m")
                if (write) append("p")
            }
        } else {
            buildString {
                if (write) append("a")
                if (write) append("d")
                if (write) append("f")
                if (read) append("r")
                if (write) append("w")
            }
        }
    }

    private fun formatUnixListDate(millis: Long): String {
        val date = Date(millis)
        val now = Calendar.getInstance()
        val then = Calendar.getInstance().apply { time = date }
        val sixMonthsMs = 31556952L / 2 * 1000
        val age = now.timeInMillis - millis
        val recent = age >= 0 && age < sixMonthsMs
        val fmt = SimpleDateFormat(if (recent) "MMM dd HH:mm" else "MMM dd  yyyy", Locale.US)
        return fmt.format(date)
    }

    private fun checkAuth() = authenticated && user != null
    private fun dataEncryptedOk() = sslContext == null || dataProtection == DataProtection.Protected
    private fun controlEncrypted() = socket is SSLSocket

    // Android 6–9 still enable TLS 1.0 and 1.1 by default. Offer only 1.2 and 1.3,
    // and only forward-secret AEAD suites. RC4, 3DES, CBC, and RSA key transport stay off.
    private fun restrictTls(ssl: SSLSocket) {
        val allowed = ssl.supportedProtocols
            .filter { it == "TLSv1.2" || it == "TLSv1.3" }
            .toTypedArray()
        if (allowed.isEmpty()) throw IllegalStateException("TLS 1.2 or 1.3 is not available")
        ssl.enabledProtocols = allowed
        val suites = ssl.supportedCipherSuites.filter { suiteAllowed(it) }.toTypedArray()
        if (suites.isEmpty()) throw IllegalStateException("No forward-secret AEAD cipher suite is available")
        ssl.enabledCipherSuites = suites
    }

    private fun suiteAllowed(name: String): Boolean {
        val upper = name.uppercase(Locale.US)
        if (upper.contains("NULL") || upper.contains("EXPORT") || upper.contains("ANON") ||
            upper.contains("RC4") || upper.contains("3DES") || upper.contains("CBC") ||
            upper.contains("_DES_") || upper.contains("MD5")
        ) {
            return false
        }
        if (upper.startsWith("TLS_AES_") || upper.startsWith("TLS_CHACHA20_")) return true
        val aead = upper.contains("GCM") || upper.contains("CHACHA20")
        val ephemeral = upper.contains("ECDHE") || upper.contains("_DHE_")
        return aead && ephemeral
    }

    // When a certificate is loaded, do not accept credentials (or PBSZ/PROT) on a clear control channel.
    private fun requireControlTls(replyCode: Int): Boolean {
        if (sslContext == null || controlEncrypted()) return true
        send(replyCode, "SSL/TLS required on the control channel")
        return false
    }

    private fun sameTlsSession(control: SSLSocket, data: SSLSocket): Boolean {
        val controlSession = control.session ?: return false
        val dataSession = data.session ?: return false
        val controlId = controlSession.id ?: ByteArray(0)
        val dataId = dataSession.id ?: ByteArray(0)
        // TLS 1.2 resumes the cached session object, so an equal id identifies the control session.
        if (controlId.isNotEmpty() && dataId.isNotEmpty() && controlId.contentEquals(dataId)) return true
        // TLS 1.3 gives the resumed connection a new session id, so the id cannot identify
        // the control session. Disabling session creation is not a substitute: BoringSSL
        // then fails the handshake with SESSION_MAY_NOT_BE_CREATED whenever the client
        // does not present a resumable ticket, and the listing never starts. The peer
        // address is still verified by the caller.
        return controlSession.protocol == "TLSv1.3" && dataSession.protocol == "TLSv1.3"
    }

    private fun samePeer(control: InetAddress, data: InetAddress): Boolean {
        if (control == data) return true
        return canonicalHost(control).contentEquals(canonicalHost(data))
    }

    private fun canonicalHost(address: InetAddress): ByteArray {
        val bytes = address.address
        if (address is Inet6Address && bytes.size == 16) {
            val ipv4Mapped = (0..9).all { bytes[it].toInt() == 0 } &&
                bytes[10] == 0xff.toByte() && bytes[11] == 0xff.toByte()
            if (ipv4Mapped) return bytes.copyOfRange(12, 16)
        }
        return bytes
    }

    private fun bindAddress(local: InetAddress): InetAddress {
        val bytes = canonicalHost(local)
        return if (bytes.size == 4) InetAddress.getByAddress(bytes) else local
    }

    private fun checkPerm(read: Boolean, write: Boolean): Boolean {
        val u = user ?: return false
        return (!read || u.canRead) && (!write || u.canWrite)
    }

    private fun info(command: String, text: String) {
        log.info("[${user?.login}] $command: $text")
    }

    private fun send(code: Int, message: String) = sendLine("$code $message")

    private fun sessionError(message: String, error: Exception) {
        if (stopping) return
        log.error(message, error)
    }

    private fun sendLine(rawLine: String) {
        if (stopping) return
        val line = SafeText.sanitize(rawLine)
        log.debug("[$clientAddress] << $line")
        val bytes = (line + "\r\n").toByteArray(encoding)
        socket.getOutputStream().write(bytes)
        socket.getOutputStream().flush()
        writer = OutputStreamWriter(socket.getOutputStream(), encoding)
    }

    companion object {
        private val LATIN1 = Charset.forName("ISO-8859-1")
        private const val MAX_COMMAND_LINE_LENGTH = 8192
        private const val UNAUTHENTICATED_IDLE_MS = 30_000L
        private const val LOGIN_TIMEOUT_MS = 60_000L
        private const val AUTHENTICATED_IDLE_MS = 5 * 60_000L
        private const val DATA_CONNECT_TIMEOUT_MS = 30_000L
        private const val DATA_IDLE_TIMEOUT_MS = 60_000L
        private val MLS_FACTS = listOf("type", "size", "modify", "perm")

        // NOOP and FEAT need no login, so an idle timer alone lets a client that never logs in
        // keep a connection slot forever. Before login the budget also ends at a fixed deadline
        // counted from connect. Zero or less means the deadline has passed.
        private fun unauthenticatedReadTimeoutMs(sinceConnectMs: Long): Long =
            minOf(UNAUTHENTICATED_IDLE_MS, LOGIN_TIMEOUT_MS - sinceConnectMs)
        private fun mlsType(name: String, isDir: Boolean) = when (name) {
            "." -> "cdir"
            ".." -> "pdir"
            else -> if (isDir) "dir" else "file"
        }
    }
}
