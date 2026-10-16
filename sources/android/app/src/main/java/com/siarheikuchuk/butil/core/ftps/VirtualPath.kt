package com.siarheikuchuk.butil.core.ftps

class VirtualPath(pathsOrSegments: Iterable<String> = emptyList()) {
    private val segments: List<String> = parse(pathsOrSegments.flatMap { it.split('/', '\\') }.filter { it.isNotEmpty() })

    val parts: List<String> get() = segments

    fun append(path: String): VirtualPath {
        if (path.startsWith('/')) return VirtualPath(listOf(path))
        return VirtualPath(segments + path)
    }

    fun goUp(): VirtualPath = append("..")

    fun toFtpsPath(): String = "/" + segments.joinToString("/")

    override fun toString(): String = toFtpsPath()

    companion object {
        private val forbidden = setOf(
            ".", "..", "con", "prn", "aux", "nul",
            "com1", "com2", "com3", "com4", "com5", "com6", "com7", "com8", "com9",
            "lpt1", "lpt2", "lpt3", "lpt4", "lpt5", "lpt6", "lpt7", "lpt8", "lpt9",
        )

        private fun parse(parts: Iterable<String>): List<String> {
            val out = mutableListOf<String>()
            for (part in parts) {
                require('\u0000' !in part) { "Path contains null bytes which are not allowed." }
                when (part) {
                    "." -> continue
                    ".." -> if (out.isNotEmpty()) out.removeAt(out.lastIndex)
                    else -> {
                        require(!part.startsWith(' ') && !part.endsWith(' ')) {
                            "Path segment cannot contain space at the beginning or end."
                        }
                        require(!isReservedDeviceName(part)) { "Path segment '${part.lowercase()}' is forbidden." }
                        out += part
                    }
                }
            }
            return out
        }

        // Windows treats CON, NUL, COM1, and those names plus any extension (con.txt) as devices.
        private fun isReservedDeviceName(segment: String): Boolean {
            var name = segment.trimEnd('.').lowercase()
            val dot = name.indexOf('.')
            if (dot >= 0) name = name.substring(0, dot)
            return name in forbidden
        }
    }
}
