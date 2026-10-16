package com.siarheikuchuk.butil.core.ftps

import java.net.InetAddress
import java.net.InetSocketAddress
import java.net.ServerSocket
import java.util.concurrent.Executors
import java.util.concurrent.atomic.AtomicBoolean
import java.util.concurrent.atomic.AtomicInteger
import javax.net.ssl.SSLContext

class FtpsServer(
    private val log: FtpsLog,
    private val config: FtpsServerConfig,
    private val fileSystem: FileSystemProvider,
    private val certificate: LoadedCertificate?,
) {
    private val running = AtomicBoolean(false)
    private val active = AtomicInteger(0)
    private val pool = Executors.newCachedThreadPool()
    private val loginThrottle = LoginThrottle()
    private val gate = Any()
    private val sessions = mutableSetOf<FtpsClientSession>()
    private val listeners = mutableListOf<ServerSocket>()
    private var maxConnections = 10

    val loadedCertificate: LoadedCertificate? get() = certificate

    fun start() {
        val requestedMax = config.settings.maxConnections
        maxConnections = FtpsServerSettings.effectiveMaxConnections(requestedMax)
        if (requestedMax != maxConnections) {
            log.warn("Max connections $requestedMax is outside 1..${FtpsServerSettings.MAX_CONNECTIONS_UPPER_BOUND}. Using $maxConnections.")
        }
        val port = config.settings.port
        openListeners(config.settings.ip, port)
        running.set(true)
        val bound = listeners.joinToString(" and ") { it.localSocketAddress?.toString() ?: config.settings.ip }
        log.info("FTPS Server started successfully on $bound (Explicit encryption)")
        for (socket in listeners.toList()) {
            pool.execute { acceptLoop(socket) }
        }
    }

    // 0.0.0.0 and :: listen on every interface. IPv6 is opened first. On a dual-stack
    // socket that already accepts IPv4, the IPv4 bind fails and that one socket serves both.
    private fun openListeners(ip: String, port: Int) {
        if (!isWildcard(ip)) {
            listeners += ServerSocket(port, 50, InetAddress.getByName(ip))
            return
        }

        try {
            val v6 = ServerSocket()
            v6.reuseAddress = true
            v6.bind(InetSocketAddress(InetAddress.getByName("::"), port), 50)
            listeners += v6
        } catch (e: Exception) {
            log.warn("IPv6 listen on [::]:$port failed (${e.message}).")
        }

        try {
            listeners += ServerSocket(port, 50, InetAddress.getByName("0.0.0.0"))
        } catch (e: Exception) {
            if (listeners.isEmpty()) throw e
            log.info("IPv4 shares the IPv6 socket on port $port.")
        }

        if (listeners.isEmpty()) error("Could not bind a listen socket.")
    }

    private fun isWildcard(ip: String): Boolean {
        val text = ip.trim()
        return text.isEmpty() || text == "0.0.0.0" || text == "::" || text == "::0"
    }

    fun stop() {
        val open: List<FtpsClientSession>
        synchronized(gate) {
            running.set(false)
            open = sessions.toList()
        }
        for (socket in listeners.toList()) {
            try {
                socket.close()
            } catch (_: Exception) {
            }
        }
        listeners.clear()
        for (session in open) session.close()
        log.info("Server stopped")
    }

    private fun acceptLoop(server: ServerSocket) {
        while (running.get()) {
            try {
                val client = server.accept()
                client.keepAlive = true
                client.tcpNoDelay = true
                val ipKey = client.inetAddress?.let { LoginThrottle.key(it) }
                if (ipKey != null && loginThrottle.isLocked(ipKey, System.currentTimeMillis())) {
                    log.warn("Connection rejected from ${client.remoteSocketAddress}: too many failed logins")
                    client.close()
                    continue
                }
                if (active.get() >= maxConnections) {
                    log.warn("Connection rejected from ${client.inetAddress}: Max connections reached")
                    client.close()
                    continue
                }
                active.incrementAndGet()
                log.info("Client connected: ${client.remoteSocketAddress} (Active: ${active.get()})")
                val session = FtpsClientSession(
                    log = log,
                    socket = client,
                    users = config.users,
                    sslContext = certificate?.sslContext,
                    fileSystem = fileSystem,
                    loginThrottle = loginThrottle,
                )
                val started = synchronized(gate) {
                    if (!running.get()) {
                        false
                    } else {
                        sessions.add(session)
                        true
                    }
                }
                if (!started) {
                    active.decrementAndGet()
                    try { client.close() } catch (_: Exception) {}
                    log.info("Client disconnected: ${client.remoteSocketAddress} (Active: ${active.get()})")
                    continue
                }
                pool.execute {
                    try {
                        session.handle()
                    } finally {
                        synchronized(gate) { sessions.remove(session) }
                        active.decrementAndGet()
                        log.info("Client disconnected: ${client.remoteSocketAddress} (Active: ${active.get()})")
                    }
                }
            } catch (e: Exception) {
                if (running.get()) log.error("Error accepting client", e)
            }
        }
    }
}
