using System;
using BUtil.Core.ButilImage;

namespace BUtil.RestorationMaster
{
	/// <summary>
	/// Restoration task information
	/// </summary>
	public class RestoreTaskInfo
	{
		readonly MetaRecord _record;
		readonly string _parameter;
		readonly RestoreType _restoreType;
			
		public RestoreType RestorationType
		{
			get { return _restoreType; }
		}

		public MetaRecord Record
		{
			get { return _record; }
		}
			
		public string Parameter
		{
			get { return _parameter; }
		}
			
		public RestoreTaskInfo(MetaRecord record, RestoreType restoreType, string parameter)
		{
			_record = record;
			_parameter = parameter;
			_restoreType = restoreType;
		}
	}
}
