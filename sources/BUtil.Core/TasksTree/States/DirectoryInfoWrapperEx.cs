using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BUtil.Core.TasksTree
{
    class DirectoryInfoWrapperEx : DirectoryInfoWrapper
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryInfoWrapperEx(DirectoryInfo directoryInfo) : base(directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

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
                    if (fileSystemInfo is DirectoryInfo directoryInfo)
                    {
                        yield return new DirectoryInfoWrapperEx(directoryInfo);
                    }
                    else
                    {
                        yield return new FileInfoWrapper((FileInfo)fileSystemInfo);
                    }
                }
            }
        }
    }
}
