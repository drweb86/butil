using System;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.IO;

	/// <summary>
	/// The compression to an archive of a CompressionItem
	/// </summary>
	internal sealed class CompressionJob: IJob
	{
		#region Fields
		
		readonly StringBuilder _compressionOutput = new StringBuilder();
		EventHandler<JobThreadEventArgs> _finished;

		#endregion
		
		#region Properties
		
		public EventHandler NotificationEventHandler = null;
		
		EventHandler<JobThreadEventArgs> IJob.TaskFinished
		{
			get { return _finished; }
			set { _finished = value; }
		}
		
		#endregion
		#region Public methods
		
		void outputReadToEnd()
		{
			
		}
		
		public void DoJob()
		{
			using (Process compressionProcess = createCompressProcess("a \"d:\\temp\\butil_tmp_0.7z\" \"E:\\DOCUMENTS\\Документация\\doctorweb.zoo.by\" -w\"d:\\temp\" -mX5 -pxxx -mhe"))
			{
				try
				{
					compressionProcess.Start();
					
					Console.WriteLine(compressionProcess.StandardOutput.ReadToEnd());
					Console.WriteLine(compressionProcess.StandardError.ReadToEnd());
					
					Thread thread = new Thread(new ParameterizedThreadStart(readOutput));
					thread.Start(compressionProcess.StandardOutput);
					
//					compressionProcess.WaitForExit();
					// FIXME: This is a workaround of .Net 2 bugs with processing ThreadAbortException and ThreadInterruptException
					// both bugs were posted to MS
					// this code is subject to be removed when those bugs will be fixed
					while (!compressionProcess.HasExited)
					{
						Thread.Sleep(1000);
					}
				}
				catch (ThreadAbortException)
				{
					compressionProcess.Kill();
				}
				finally
				{
					if (_finished != null)
						_finished.Invoke(this, new JobThreadEventArgs(Thread.CurrentThread));
				}
			}
		}
		
		#endregion
			void readOutput(object obj)
		{
			StreamReader writer = (StreamReader)obj;
			Console.WriteLine( writer.ReadToEnd() );
		}
		
		#region Private Methods

		static Process createCompressProcess(string arguments)
		{
			Process result = new Process();
			result.StartInfo.UseShellExecute = false;
			result.StartInfo.RedirectStandardOutput = true;
			result.StartInfo.RedirectStandardError = true;
			result.StartInfo.CreateNoWindow = true;
			result.StartInfo.FileName = @"E:\Work\BUtil\BUtil-workingCopy\7-zip\7z.exe";
			result.StartInfo.Arguments = arguments;
			
			return result;
		}
				
		void notify(EventArgs e)
		{
			if (NotificationEventHandler != null)
			{
				NotificationEventHandler.Invoke(this, e);
			}
		}
		
		#endregion
	}
