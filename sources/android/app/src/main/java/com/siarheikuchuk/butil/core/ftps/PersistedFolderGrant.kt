package com.siarheikuchuk.butil.core.ftps

import android.content.Context
import android.content.Intent
import android.net.Uri
import android.provider.DocumentsContract
import androidx.activity.result.contract.ActivityResultContracts
import java.io.FileNotFoundException

/**
 * A shared folder is a persistable tree grant, not a path. The grant is stored by the
 * system and can be dropped or rewritten (different authority, same document id) when
 * the phone is updated. The string in settings is only a hint.
 */
object PersistedFolderGrant {
    private const val READ = Intent.FLAG_GRANT_READ_URI_PERMISSION
    private const val WRITE = Intent.FLAG_GRANT_WRITE_URI_PERMISSION

    /**
     * Persists read and write for [uri], including children of the tree.
     * Returns the URI string the system actually kept, or null when it did not.
     */
    fun take(context: Context, uri: Uri): String? {
        val resolver = context.contentResolver
        try {
            resolver.takePersistableUriPermission(uri, READ or WRITE)
        } catch (_: SecurityException) {
            return null
        }
        val kept = resolver.persistedUriPermissions.firstOrNull {
            it.isReadPermission && it.isWritePermission && sameTree(it.uri, uri)
        } ?: return null
        if (!canReadTree(context, kept.uri)) {
            releaseExact(context, kept.uri.toString())
            return null
        }
        return kept.uri.toString()
    }

    /**
     * The persisted URI to use for [stored]. Matches a grant whose tree document id is
     * the same even when a system update changed the authority.
     */
    fun liveUri(context: Context, stored: String, needsWrite: Boolean): String? {
        if (stored.isBlank()) return null
        val storedUri = Uri.parse(stored)
        val matches = context.contentResolver.persistedUriPermissions.filter { perm ->
            perm.isReadPermission &&
                (!needsWrite || perm.isWritePermission) &&
                sameTree(perm.uri, storedUri)
        }
        val ordered = matches.sortedByDescending { it.uri == storedUri }
        return ordered.firstOrNull { canReadTree(context, it.uri) }?.uri?.toString()
    }

    fun releaseUnused(context: Context, uriString: String, stillUsed: Collection<String>) {
        if (uriString.isBlank() || uriString in stillUsed) return
        releaseExact(context, uriString)
    }

    /** Document URI for reopening the system picker on the previously shared folder. */
    fun pickerUri(stored: String): Uri? {
        if (stored.isBlank()) return null
        val uri = Uri.parse(stored)
        val docId = treeDocumentId(uri) ?: return uri
        return try {
            DocumentsContract.buildDocumentUriUsingTree(uri, docId)
        } catch (_: Exception) {
            uri
        }
    }

    private fun releaseExact(context: Context, uriString: String) {
        val uri = Uri.parse(uriString)
        val perm = context.contentResolver.persistedUriPermissions.firstOrNull { it.uri == uri } ?: return
        var flags = 0
        if (perm.isReadPermission) flags = flags or READ
        if (perm.isWritePermission) flags = flags or WRITE
        if (flags == 0) return
        try {
            context.contentResolver.releasePersistableUriPermission(perm.uri, flags)
        } catch (_: SecurityException) {
        }
    }

    private fun canReadTree(context: Context, treeUri: Uri): Boolean {
        val docId = treeDocumentId(treeUri) ?: return false
        return try {
            // A cursor with no rows is an empty folder. Null is inconclusive.
            // SecurityException means this grant does not cover the children.
            val children = DocumentsContract.buildChildDocumentsUriUsingTree(treeUri, docId)
            context.contentResolver.query(
                children,
                arrayOf(DocumentsContract.Document.COLUMN_DOCUMENT_ID),
                null,
                null,
                null,
            )?.use { true } ?: true
        } catch (_: SecurityException) {
            false
        } catch (_: FileNotFoundException) {
            false
        } catch (_: Exception) {
            true
        }
    }

    private fun sameTree(a: Uri, b: Uri): Boolean {
        if (a == b) return true
        val aId = treeDocumentId(a)
        val bId = treeDocumentId(b)
        return aId != null && aId == bId
    }

    private fun treeDocumentId(uri: Uri): String? {
        val segments = uri.pathSegments
        if (segments.size < 2 || segments[0] != "tree") return null
        return try {
            DocumentsContract.getTreeDocumentId(uri)
        } catch (_: IllegalArgumentException) {
            null
        }
    }
}

/** Asks DocumentsUI for a persistable grant that covers every file under the chosen folder. */
class PersistableOpenDocumentTree : ActivityResultContracts.OpenDocumentTree() {
    override fun createIntent(context: Context, input: Uri?): Intent {
        return super.createIntent(context, input).addFlags(
            Intent.FLAG_GRANT_READ_URI_PERMISSION or
                Intent.FLAG_GRANT_WRITE_URI_PERMISSION or
                Intent.FLAG_GRANT_PERSISTABLE_URI_PERMISSION or
                Intent.FLAG_GRANT_PREFIX_URI_PERMISSION,
        )
    }
}
