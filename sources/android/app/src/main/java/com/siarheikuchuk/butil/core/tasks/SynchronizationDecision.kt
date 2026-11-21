package com.siarheikuchuk.butil.core.tasks

/**
 * Port of desktop SynchronizationDecisionService. Keep the action rules aligned with
 * sources/BUtil.Tasks.Synchronization/Synchronization/SynchronizationDecisionService.cs.
 */
internal enum class SyncRelation { NotChanged, Created, Changed, Deleted }

internal enum class SyncAction { DoNothing, Update, Delete }

internal data class SyncFile(
    val relativeFileName: String,
    val modifiedAtUtcMillis: Long,
    val sha512: String,
    val size: Long,
) {
    fun sameContent(other: SyncFile?): Boolean {
        if (other == null) return false
        return size == other.size && sha512.equals(other.sha512, ignoreCase = true)
    }
}

internal class SyncItem(val relativeFileName: String) {
    var actual: SyncFile? = null
    var local: SyncFile? = null
    var remote: SyncFile? = null
    var actualToLocal = SyncRelation.NotChanged
    var remoteToLocal = SyncRelation.NotChanged
    var actualAction = SyncAction.DoNothing
    var remoteAction = SyncAction.DoNothing
    var forceUpdateState = false
    val existsLocally: Boolean get() = actual != null
}

internal object SynchronizationDecisionService {
    fun decide(
        synchronizationMode: Int,
        localState: List<SyncFile>,
        actualFiles: List<SyncFile>,
        remoteState: List<SyncFile>,
    ): List<SyncItem> {
        val items = unionStates(actualFiles, localState, remoteState)
        buildRelations(items)
        resolveActions(items, synchronizationMode)
        return items
    }

    private fun resolveActions(items: List<SyncItem>, synchronizationMode: Int) {
        items.forEach { resolveAction(it, synchronizationMode) }
    }

    private fun resolveAction(item: SyncItem, synchronizationMode: Int) {
        item.actualAction = SyncAction.DoNothing
        item.remoteAction = SyncAction.DoNothing
        val actualFile = item.actual
        val remoteFile = item.remote
        if (actualFile != null && remoteFile != null && actualFile.sameContent(remoteFile)) {
            item.forceUpdateState = !remoteFile.sameContent(item.local)
            return
        }
        when (synchronizationMode) {
            SynchronizationTask.MODE_TWO_WAY -> resolveActionTwoWayMode(item)
            SynchronizationTask.MODE_READ -> resolveActionReadonlyMode(item)
            SynchronizationTask.MODE_WRITE -> resolveActionWriteMode(item)
        }
    }

    private fun resolveActionTwoWayMode(item: SyncItem) {
        item.actualAction = SyncAction.DoNothing
        item.remoteAction = SyncAction.DoNothing
        val actualFile = item.actual
        val remoteFile = item.remote
        val actualRelation = item.actualToLocal
        val remoteRelation = item.remoteToLocal
        if (item.existsLocally && item.remote == null && item.local == null &&
            actualRelation == SyncRelation.Created && remoteRelation == SyncRelation.NotChanged
        ) {
            item.remoteAction = SyncAction.Update
            return
        }
        if (item.existsLocally && item.remote != null && item.local != null &&
            actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.NotChanged
        ) {
            return
        }
        if (item.existsLocally && item.remote != null && item.local != null &&
            actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.Changed
        ) {
            item.actualAction = SyncAction.Update
            return
        }
        if (!item.existsLocally && item.remote != null &&
            actualRelation == SyncRelation.Deleted && remoteRelation == SyncRelation.NotChanged
        ) {
            item.remoteAction = SyncAction.Delete
            return
        }
        if (item.existsLocally && actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.Deleted) {
            item.actualAction = SyncAction.Delete
            return
        }
        if (remoteFile != null && actualFile != null && item.existsLocally && actualRelation == SyncRelation.Created &&
            (remoteRelation == SyncRelation.Created || remoteRelation == SyncRelation.Changed)
        ) {
            if (remoteFile.modifiedAtUtcMillis > actualFile.modifiedAtUtcMillis) item.actualAction = SyncAction.Update
            else item.remoteAction = SyncAction.Update
            return
        }
        if (item.existsLocally && actualRelation == SyncRelation.Created && remoteRelation == SyncRelation.Deleted) {
            item.remoteAction = SyncAction.Update
        }
        if (item.existsLocally && actualRelation == SyncRelation.Changed && remoteRelation == SyncRelation.NotChanged) {
            item.remoteAction = SyncAction.Update
            return
        }
        if (remoteFile != null && actualFile != null && item.existsLocally &&
            actualRelation == SyncRelation.Changed && remoteRelation == SyncRelation.Changed
        ) {
            if (remoteFile.modifiedAtUtcMillis > actualFile.modifiedAtUtcMillis) item.actualAction = SyncAction.Update
            else item.remoteAction = SyncAction.Update
            return
        }
        if (item.existsLocally && actualRelation == SyncRelation.Changed && remoteRelation == SyncRelation.Deleted) {
            item.remoteAction = SyncAction.Update
            return
        }
        if (!item.existsLocally && actualRelation == SyncRelation.Deleted && remoteRelation == SyncRelation.Changed) {
            item.actualAction = SyncAction.Update
            return
        }
        if (!item.existsLocally && actualRelation == SyncRelation.Deleted && remoteRelation == SyncRelation.Deleted) {
            item.forceUpdateState = true
            return
        }
        if (!item.existsLocally && actualRelation == SyncRelation.NotChanged && remoteRelation == SyncRelation.Created) {
            item.actualAction = SyncAction.Update
        }
    }

    private fun resolveActionReadonlyMode(item: SyncItem) {
        fun apply(relation: SyncRelation, changed: SyncAction, deleted: SyncAction = SyncAction.DoNothing, created: SyncAction = SyncAction.Update) {
            item.actualAction = when (relation) {
                SyncRelation.NotChanged -> SyncAction.DoNothing
                SyncRelation.Created, SyncRelation.Changed -> created
                SyncRelation.Deleted -> deleted
            }
        }
        when (item.actualToLocal) {
            SyncRelation.NotChanged -> apply(item.remoteToLocal, SyncAction.Update, SyncAction.Delete)
            SyncRelation.Created -> when (item.remoteToLocal) {
                SyncRelation.NotChanged -> item.actualAction = SyncAction.DoNothing
                SyncRelation.Created -> item.actualAction = SyncAction.Update
                else -> Unit
            }
            SyncRelation.Changed -> when (item.remoteToLocal) {
                SyncRelation.NotChanged, SyncRelation.Changed -> item.actualAction = SyncAction.Update
                SyncRelation.Deleted -> item.actualAction = SyncAction.Delete
                SyncRelation.Created -> Unit
            }
            SyncRelation.Deleted -> when (item.remoteToLocal) {
                SyncRelation.NotChanged, SyncRelation.Changed -> item.actualAction = SyncAction.Update
                SyncRelation.Deleted -> item.actualAction = SyncAction.DoNothing
                SyncRelation.Created -> Unit
            }
        }
    }

    private fun resolveActionWriteMode(item: SyncItem) {
        item.actualAction = SyncAction.DoNothing
        item.remoteAction = SyncAction.DoNothing
        val actualFile = item.actual
        if (actualFile != null) {
            if (item.remote == null || !actualFile.sameContent(item.remote)) {
                item.remoteAction = SyncAction.Update
            }
            return
        }
        if (item.remote != null) {
            item.remoteAction = SyncAction.Delete
            return
        }
        item.forceUpdateState = item.local != null
    }

    private fun buildRelations(items: List<SyncItem>) {
        items.forEach { item ->
            item.actualToLocal = relation(item.actual, item.local)
            item.remoteToLocal = relation(item.remote, item.local)
        }
    }

    private fun unionStates(
        actualFiles: List<SyncFile>,
        localState: List<SyncFile>,
        remoteState: List<SyncFile>,
    ): List<SyncItem> {
        val items = linkedMapOf<String, SyncItem>()
        fun add(files: List<SyncFile>, assign: (SyncItem, SyncFile) -> Unit) {
            files.forEach { file ->
                val item = items.getOrPut(file.relativeFileName) { SyncItem(file.relativeFileName) }
                assign(item, file)
            }
        }
        add(actualFiles) { item, file -> item.actual = file }
        add(localState) { item, file -> item.local = file }
        add(remoteState) { item, file -> item.remote = file }
        return items.values.toList()
    }

    private fun relation(primary: SyncFile?, secondary: SyncFile?): SyncRelation {
        if (primary == null) return if (secondary == null) SyncRelation.NotChanged else SyncRelation.Deleted
        if (secondary == null) return SyncRelation.Created
        return if (primary.sameContent(secondary)) SyncRelation.NotChanged else SyncRelation.Changed
    }
}
