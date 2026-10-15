package com.siarheikuchuk.butil.core.storage

import java.io.InputStream
import java.io.OutputStream

interface Storage : AutoCloseable {
    fun upload(relativeFileName: String, length: Long, open: () -> InputStream)
    fun exists(relativeFileName: String): Boolean
    fun deleteFolder(relativeFolderName: String)
    fun test(): String?

    override fun close() {}
}

object StorageIo {
    fun copy(input: InputStream, output: OutputStream) {
        val buffer = ByteArray(1024 * 256)
        while (true) {
            val read = input.read(buffer)
            if (read < 0) break
            output.write(buffer, 0, read)
        }
        output.flush()
    }
}
