#nullable disable
/* Adopted to the needs of project
 * with some portions of my code
 * (see license)
 */

using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Data;
using System.Threading;

namespace BUtil.Core.Storages
{
	public class FtpConnection
	{
		const string _SERVERLOCATION = "ServerLocation";
        const string _LOGIN = "login";
        const string _PASSWORD = "password";
        const string _ISNULLOREMPTY = "Not set or empty";

		string ftpServerIP;
        string ftpUserID;
        string ftpPassword;
        bool isPassive = true;
        
        public bool IsPassive
        {
        	get { return isPassive; }
        	set 
        	{ isPassive = value; }
        }
        
        public string ServerLocation
        {
        	get { return ftpServerIP; }
        	set 
        	{
                if (string.IsNullOrEmpty(value)) 
					throw new ArgumentNullException(_SERVERLOCATION);

        		ftpServerIP = value;
        	}
        }
        
		public void SetLogOnInformation(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) 
				throw new ArgumentNullException(_LOGIN);
            if (string.IsNullOrEmpty(password)) 
				throw new ArgumentNullException(_PASSWORD);
			
            ftpUserID = login;
            ftpPassword = password;
        }

		/// <summary>
        /// Method to upload the specified file to the specified FTP Server
        /// </summary>
        /// <param name="filename">file full name to be uploaded</param>
        public void Upload(string ftpFolder, string fileName)
        {
			FileInfo fileInf = new FileInfo(fileName);
            FtpWebRequest reqFTP = LogOnHelper(ftpFolder + "/" + fileInf.Name);
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Notify the server about the size of the uploaded file
            reqFTP.ContentLength = fileInf.Length;

            // The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            FileStream fs = null;
            Stream strm = null;
            try
            {
                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                fs = fileInf.OpenRead();

                // Stream to which the file to be upload is written
                strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
            }
            finally
            {
                // Close the file stream and the Request Stream
                if (strm != null) strm.Close();
                if (fs != null) fs.Close();
            }
        }
        
		public void DeleteFtp(string fileName)
        {
            FtpWebRequest reqFTP = LogOnHelper(fileName);
            reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = null;
            Stream datastream = null;
            StreamReader sr = null;
            try
            {
                response = (FtpWebResponse)reqFTP.GetResponse();
                datastream = response.GetResponseStream();
                sr = new StreamReader(datastream);
                sr.ReadToEnd();
            }
            finally
            {
                if (sr != null) sr.Close();
                if (datastream != null) datastream.Close();
                if (response != null) response.Close();
            }
        }
        

		public string[] GetFileList(string path)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP = LogOnHelper(path);
            WebResponse response = null;
            StreamReader reader = null;

            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (response != null) response.Close();
            }

            return result.ToString().Split('\n');
        }

        #region Helper methods

        private FtpWebRequest LogOnHelper(string path)
        {
            FtpWebRequest reqFtp;
#pragma warning disable SYSLIB0014 // Type or member is obsolete
            reqFtp = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + path));
#pragma warning restore SYSLIB0014 // Type or member is obsolete
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFtp.UsePassive = isPassive;
            return reqFtp;
        }

        #endregion

    }
}
