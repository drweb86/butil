using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BULocalization
{

	[Serializable]
	public class LocalizationGenericException: Exception
	{
		private string _details = string.Empty;
		
		/// <summary>
		/// Message property of original exception
		/// </summary>
		public string Details
		{
			get { return _details; }
		}
		
		#region Constructors
		
		public LocalizationGenericException(string message, string details)
			: base(message)
		{
			_details = details;
		}
		
		public LocalizationGenericException(string message)
			: base(message)
		{
			;
		}
		
		protected LocalizationGenericException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
                throw new System.ArgumentNullException("info");
			
			_details = info.GetString("_details");
		}
				
		public LocalizationGenericException()
			: base()
		{
			;
		}
		
		public LocalizationGenericException(string message, Exception innerException)
			: base(message, innerException)
		{
			;
		}
		
		#endregion
		
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            
            info.AddValue("_details", _details);
            base.GetObjectData(info, context);
        }
	}
}
