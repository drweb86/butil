using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.Core.TasksTree;

class DirectoryInfoWrapperEx(DirectoryInfo directoryInfo) : DirectoryInfoWrapper(directoryInfo)
{
    private readonly DirectoryInfo _directoryInfo = directoryInfo;
    private readonly List<string> _skipDirectories = ["System Volume Information", "$RECYCLE.BIN", "Recovery"];

    public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos()
    {
        if (_directoryInfo.Exists)
        {
            IEnumerable<FileSystemInfo> fileSystemInfos;
            try
            {
                fileSystemInfos = _directoryInfo.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }

            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
            {
                if (_skipDirectories.Contains(fileSystemInfo.Name))
                    continue;

                if (fileSystemInfo is DirectoryInfo directoryInfo)
                    yield return new DirectoryInfoWrapperEx(directoryInfo);
                else
                    yield return new FileInfoWrapper((FileInfo)fileSystemInfo);
            }
        }
    }
}
