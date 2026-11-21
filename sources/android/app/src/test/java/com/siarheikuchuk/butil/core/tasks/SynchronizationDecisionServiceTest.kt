package com.siarheikuchuk.butil.core.tasks

import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test

class SynchronizationDecisionServiceTest {
    @Test
    fun writePublishesLocalFolder() {
        val time = 1_700_000_000_000L
        val local = listOf(
            file("keep.txt", time, "sha-keep", 1),
            file("changed.txt", time, "sha-old", 1),
            file("gone.txt", time, "sha-gone", 1),
        )
        val actual = listOf(
            file("keep.txt", time, "sha-keep", 1),
            file("changed.txt", time, "sha-local", 2),
            file("only-local.txt", time, "sha-new", 3),
        )
        val remote = listOf(
            file("keep.txt", time, "sha-keep", 1),
            file("changed.txt", time + 5 * 60 * 60 * 1000, "sha-remote", 4),
            file("only-remote.txt", time + 5 * 60 * 60 * 1000, "sha-remote-only", 5),
        )

        val decisions = SynchronizationDecisionService.decide(SynchronizationTask.MODE_WRITE, local, actual, remote)

        assertTrue(decisions.all { it.actualAction == SyncAction.DoNothing })
        assertRemote("keep.txt", decisions, SyncAction.DoNothing)
        assertRemote("changed.txt", decisions, SyncAction.Update)
        assertRemote("only-local.txt", decisions, SyncAction.Update)
        assertRemote("only-remote.txt", decisions, SyncAction.Delete)
        val gone = decisions.single { it.relativeFileName == "gone.txt" }
        assertEquals(SyncAction.DoNothing, gone.remoteAction)
        assertTrue(gone.forceUpdateState)
        assertEquals(
            listOf("changed.txt", "keep.txt", "only-local.txt"),
            decisions.filter { it.existsLocally && it.remoteAction != SyncAction.Delete }
                .map { it.relativeFileName }
                .sorted(),
        )
    }

    @Test
    fun writeKeepsLocalFileWhenRemoteIsNewer() {
        val time = 1_700_000_000_000L
        val local = listOf(file("a.txt", time, "sha-local", 1))
        val actual = listOf(file("a.txt", time, "sha-local", 1))
        val remote = listOf(file("a.txt", time + 2 * 24 * 60 * 60 * 1000, "sha-remote", 9))

        val item = SynchronizationDecisionService.decide(SynchronizationTask.MODE_WRITE, local, actual, remote).single()

        assertEquals(SyncAction.DoNothing, item.actualAction)
        assertEquals(SyncAction.Update, item.remoteAction)
    }

    @Test
    fun readDownloadsANewerRemoteEdit() {
        val time = 1_700_000_000_000L
        val local = listOf(file("a.txt", time, "sha-local", 1))
        val actual = listOf(file("a.txt", time, "sha-local", 1))
        val remote = listOf(file("a.txt", time + 1_000, "sha-remote", 9))

        val item = SynchronizationDecisionService.decide(SynchronizationTask.MODE_READ, local, actual, remote).single()

        assertEquals(SyncAction.Update, item.actualAction)
        assertEquals(SyncAction.DoNothing, item.remoteAction)
    }

    @Test
    fun modeNumbersMatchDesktop() {
        assertEquals(0, SynchronizationTask.MODE_TWO_WAY)
        assertEquals(1, SynchronizationTask.MODE_READ)
        assertEquals(2, SynchronizationTask.MODE_WRITE)
        assertEquals(SynchronizationTask.MODE_READ, SynchronizationTask().synchronizationMode)
    }

    private fun file(name: String, modified: Long, sha: String, size: Long) = SyncFile(name, modified, sha, size)

    private fun assertRemote(name: String, decisions: List<SyncItem>, action: SyncAction) {
        assertEquals(action, decisions.single { it.relativeFileName == name }.remoteAction)
    }
}
