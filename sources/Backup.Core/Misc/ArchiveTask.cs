using System;
using System.Threading;
using System.Diagnostics;
using BUtil.Core.Options;
using BUtil.Core.ButilImage;
using BUtil.Core.FileSystem;

[assembly: CLSCompliant(true)]
namespace BUtil.Core.Misc
{
		/// <summary>
		/// Description of ArchiveParameter.
		/// </summary>
		public sealed class ArchiveTask
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
			
			void сreateArgumentsForCompression()
			{
				char compressionDegree;
				switch (_compressionItem.CompressionDegree)
				{
					case CompressionDegree.Store : compressionDegree = '0'; break;
					case CompressionDegree.Fastest: compressionDegree = '1'; break;
					case CompressionDegree.Fast: compressionDegree = '3'; break;
					case CompressionDegree.Normal: compressionDegree = '5'; break;
					case CompressionDegree.Maximum: compressionDegree = '7'; break;
					case CompressionDegree.Ultra: compressionDegree = '9'; break;
					default: throw new InvalidOperationException("Invalid CompressionDegree specified!");
				}

				_arguments = "a \"" + ArchiveName + "\" \"" + _compressionItem.Target + "\" -w\"" + Directories.TempFolder + "\" -mX" + compressionDegree;
                _logEntry = _arguments;
			}
			
			void сreateArgumentsForCompressionWithPassword(string password)
			{
				сreateArgumentsForCompression();
				_arguments += " -p" + password + " -mhe";
                _logEntry += " -pXXXXXXXX" + " -mhe";
			}
						
			

			public ArchiveTask(ProcessPriorityClass priority, string resultArchive, SourceItem item, string password)
			{
				if (item == null)
					throw new ArgumentNullException("item");

				_priority = priority;
				_archiveName = resultArchive;
				_compressionItem = item;
				if (!string.IsNullOrWhiteSpace(password))
					сreateArgumentsForCompressionWithPassword(password);
			}

            public override string ToString()
            {
                return _logEntry;
            }
		}
}
