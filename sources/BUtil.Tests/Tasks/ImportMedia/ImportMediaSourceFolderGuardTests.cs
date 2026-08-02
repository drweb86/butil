using BUtil.Tasks.ImportMedia;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BUtil.Tests.Tasks.ImportMedia;

[TestClass]
public class ImportMediaSourceFolderGuardTests
{
    [TestMethod]
    public void TryGetTooBroadFolderError_WhenMultipleKnownFolders_ReturnsError()
    {
        var error = ImportMediaSourceFolderGuard.TryGetTooBroadFolderError(["DCIM", "Pictures", "Music"]);

        Assert.IsNotNull(error);
        Assert.IsTrue(error.Contains("DCIM"));
        Assert.IsTrue(error.Contains("Pictures"));
    }

    [TestMethod]
    public void TryGetTooBroadFolderError_WhenSingleKnownFolder_ReturnsNull()
    {
        var error = ImportMediaSourceFolderGuard.TryGetTooBroadFolderError(["DCIM", "Vacation2024"]);

        Assert.IsNull(error);
    }

    [TestMethod]
    public void TryGetTooBroadFolderError_WhenNoKnownFolders_ReturnsNull()
    {
        var error = ImportMediaSourceFolderGuard.TryGetTooBroadFolderError(["Album", "Raw"]);

        Assert.IsNull(error);
    }
}

[TestClass]
public class ImportMediaFileExtensionsTests
{
    [TestMethod]
    public void Matches_WhenNoExtensionsConfigured_AllowsAll()
    {
        Assert.IsTrue(ImportMediaFileExtensions.Matches("a.jpg", null));
        Assert.IsTrue(ImportMediaFileExtensions.Matches("a.bin", []));
    }

    [TestMethod]
    public void Parse_NormalizesAndDeduplicates()
    {
        var parsed = ImportMediaFileExtensions.Parse(".JPG, png;MP3 mp3");

        CollectionAssert.AreEqual(new[] { ".jpg", ".png", ".mp3" }, parsed);
    }
}
