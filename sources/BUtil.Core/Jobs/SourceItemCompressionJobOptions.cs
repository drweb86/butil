using System;
using System.Threading;
using System.Diagnostics;
using BUtil.Core.Options;
using BUtil.Core.ButilImage;
using BUtil.Core.FileSystem;

[assembly: CLSCompliant(true)]
namespace BUtil.Core.Jobs
{
    public sealed class SourceItemCompressionJobOptions
    {
        readonly string _archiveName;
        readonly SourceItem _compressionItem;
        ProcessPriorityClass _priority;
        string _arguments = string.Empty;
        string _logEntry = string.Empty;

        public string ArchiveName
        {
            get { return _archiveName; }
        }

        public string Arguments
        {
            get { return _arguments; }
        }

        public SourceItem ItemToCompress
        {
            get { return _compressionItem; }
        }

        public ProcessPriorityClass Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        void СreateArgumentsForCompression()
        {
            //var compressionDegree = _compressionItem.CompressionDegree switch
            //{
            //    CompressionDegree.Store => '0',
            //    CompressionDegree.Fastest => '1',
            //    CompressionDegree.Fast => '3',
            //    CompressionDegree.Normal => '5',
            //    CompressionDegree.Maximum => '7',
            //    CompressionDegree.Ultra => '9',
            //    _ => throw new InvalidOperationException("Invalid CompressionDegree specified!"),
            //};
            //_arguments = "a \"" + ArchiveName + "\" \"" + _compressionItem.Target + "\" -w\"" + Directories.TempFolder + "\" -mX" + compressionDegree;
            //_logEntry = _arguments;
        }

        void СreateArgumentsForCompressionWithPassword(string password)
        {
            СreateArgumentsForCompression();
            _arguments += " -p" + password + " -mhe";
            _logEntry += " -pXXXXXXXX" + " -mhe";
        }



        public SourceItemCompressionJobOptions(ProcessPriorityClass priority, string resultArchive, SourceItem item, string password)
        {
            _priority = priority;
            _archiveName = resultArchive;
            _compressionItem = item;
            if (!string.IsNullOrWhiteSpace(password))
                СreateArgumentsForCompressionWithPassword(password);
            else
                СreateArgumentsForCompression();
        }

        public override string ToString()
        {
            return _logEntry;
        }
    }
}
