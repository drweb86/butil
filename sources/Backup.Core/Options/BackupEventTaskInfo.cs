
using System;

namespace BUtil.Core.Options
{
	/// <summary>
	/// This is a base class for backup tasks before or after the backup
	/// </summary>
	public sealed class BackupEventTaskInfo
	{
		#region Private Fields
		
		readonly string _program;
		readonly string _arguments;
		
		#endregion
		
		#region Public Properties
		
		/// <summary>
		/// The program to run
		/// </summary>
		public string Program
		{
			get { return _program; }
		}

		/// <summary>
		/// The arguments to pass to program
		/// </summary>
		public string Arguments
		{
			get { return _arguments; }
		}
		
		#endregion
		
		#region Contructors
		
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="program">The program to run</param>
		/// <param name="arguments">The arguments to pass to program</param>
		/// <exception cref="ArgumentNullException">Program is null or empty</exception>
		public BackupEventTaskInfo(string program, string arguments)
		{
			if (string.IsNullOrEmpty(program))
				{
					throw new ArgumentNullException("program");
				}

			_program = program;
			_arguments = arguments;
		}
		
		#endregion
		
		#region Public Methods
		
		public override string ToString()
		{
			return _program + ' ' + _arguments;
		}
		
		#endregion
	}
}
