using System;
using System.Diagnostics;
using System.Threading;

namespace Tests
{
	/// <summary>
	/// Test for testing the hang up of 7-zip during compression of site
	/// </summary>
	public static class Program
	{
		
		public static void Main()
		{
			TaskManager man = new TaskManager(1);
			man.AddJob(new CompressionJob());
			
			man.WaitUntilAllJobsAreDone();

			Console.ReadLine();
		}
	}
	
	
}
