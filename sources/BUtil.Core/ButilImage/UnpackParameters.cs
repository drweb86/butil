using System;

namespace BUtil.Core.ButilImage
{
	/// <summary>
	/// Description of UnpackParameter.
	/// </summary>
	public class UnpackParameters
	{
		string _result = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="archiveToUnpackName"></param>
        /// <param name="destinationFolder">without quotes!</param>
        public UnpackParameters(string password, string archiveToUnpackName, string destinationFolder)
            : this(archiveToUnpackName, destinationFolder)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            _result = toArguments(password, archiveToUnpackName, destinationFolder);
        }

        /// <summary>
        /// without password variant
        /// </summary>
        /// <param name="archiveToUnpackName"></param>
        /// <param name="destinationFolder"></param>
        public UnpackParameters(string archiveToUnpackName, string destinationFolder)
        {
            if (string.IsNullOrEmpty(archiveToUnpackName))
                throw new ArgumentNullException("archiveToUnpackName");

            if (string.IsNullOrEmpty(destinationFolder))
                throw new ArgumentNullException("destinationFolder");
            
            _result = toArguments(string.Empty, archiveToUnpackName, destinationFolder);
        }

        public string toArguments(string password, string archiveToUnpackName, string destinationFolder)
		{
			string parameters = "x " + "\"" + archiveToUnpackName + "\" -o\"" + destinationFolder + "\" -aoa -y";

            if (string.IsNullOrEmpty(password)) 
				return parameters;
			else 
				return parameters + " -p" + password;
			
		}
        
        public override string ToString()
        {
			return _result;
        }
	}
}
