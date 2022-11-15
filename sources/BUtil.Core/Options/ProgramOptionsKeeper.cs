using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;


namespace BUtil.Core.Options
{
    public static class ProgramOptionsKeeper
    {
    	#region Xml Tags
    	
        const string _HEADER_TAG = "Settings";
        const string _PERFORMANCE_TAG = "Performance";
        const string _SECURITY_TAG = "Security";
        const string _GORE_TAG = "Core";
        const string _LOGS_TAG = "Logging";
        const string _CONFIGURATOR_TAG = "Configurator";
		
        
        #endregion

        #region Consts
        
        const string _fileInaccessibleOrCorrupted = "File with profile options '{0}' is inaccessible or corrupted: {1}";
		
        #endregion
        
        #region Private methods
        
        /// <summary>
        /// Adds text node to document to node
        /// </summary>
        /// <param name="document">The xml document</param>
        /// <param name="node">Node of xml document to which a new setting must be added</param>
        /// <param name="nodeName">The name of a new node</param>
        /// <param name="value">The value of the node</param>
		static void addTextNode(XmlDocument document, XmlNode node, string nodeName, string value)
		{
			XmlNode newNode = document.CreateNode(XmlNodeType.Element, nodeName, string.Empty);
			newNode.InnerText = value;
			node.AppendChild(newNode);
		}
		
        /// <summary>
        /// Adds text attribute to document to node
        /// </summary>
        /// <param name="document">The xml document</param>
        /// <param name="node">Node of xml document to which a new setting must be added</param>
        /// <param name="attributeName">The name of a new attribute</param>
        /// <param name="value">The value of the attribute</param>
		static void addAttributeToNode(XmlDocument document, XmlNode node, string attributeName, string value)
		{
			XmlAttribute attribute = document.CreateAttribute(attributeName);
			attribute.Value = value;
			node.Attributes.Append(attribute);
		}
		
		/// <summary>
		/// Reads string value from Node
		/// </summary>
		/// <param name="document">The Xml Document</param>
		/// <param name="xPath">XPath to node</param>
		/// <param name="defaultValue">The Default value that will be returned in case node not found</param>
		/// <returns>The setting</returns>
        static string readNode(XmlDocument document, string xPath, string defaultValue)
        {
        	XmlNode node = document.SelectSingleNode(xPath);
			if (node != null)
			{
				return node.InnerText;
			}
			else
			{
				Debug.WriteLine(xPath + " : Missing");
				return defaultValue;
			}
        }

        /// <summary>
        /// Reads an integer value from xml document
        /// </summary>
        /// <param name="document">The xml document</param>
        /// <param name="xPath">The XPath to setting</param>
        /// <param name="minimumValue">Minimum expected value</param>
        /// <param name="maximumValue">Maximum expected value</param>
        /// <param name="defaultValue">This value will be returned in case setting is missing or out of range</param>
        /// <returns>The setting</returns>
        static int readNode(XmlDocument document, string xPath, int minimumValue, int maximumValue, int defaultValue)
        {
        	XmlNode node = document.SelectSingleNode(xPath);
			if (node != null)
			{
				int result = defaultValue;
				if (Int32.TryParse(node.InnerText, out result))
				{
					if (result < minimumValue || result > maximumValue)
					{
						Debug.WriteLine(xPath + " : Out of range");
						return defaultValue;
					}
					else
					{
						return result;
					}
				}
				else
				{
					Debug.WriteLine(xPath + " : Invalid");
					return defaultValue;
				}
			}
			else
			{
				Debug.WriteLine(xPath + " : Missing");
				return defaultValue;
			}
        }
        
        /// <summary>
        /// Reads boolean setting from xml document
        /// </summary>
        /// <param name="document">The Xml Document</param>
        /// <param name="xPath">The target xpath</param>
        /// <param name="defaultValue">Default value that will be returned in case setting is missing</param>
        /// <returns>The setting</returns>
        static bool readNode(XmlDocument document, string xPath, bool defaultValue)
        {
        	XmlNode node = document.SelectSingleNode(xPath);
			if (node != null)
			{
				bool result = defaultValue;
				if (bool.TryParse(node.InnerText, out result))
				{
					return result;
				}
				else
				{
					Debug.WriteLine(xPath + " : Invalid");
					return defaultValue;
				}
			}
			else
			{
				Debug.WriteLine(xPath + " : Missing");
				return defaultValue;
			}
        }
		
		#endregion
		
		#region Public Methods
		
        public static void StoreSettings(ProgramOptions options)
        {
            try
            {
				XmlDocument document = new XmlDocument();
				XmlNode header = document.CreateNode(XmlNodeType.Element, _HEADER_TAG, string.Empty);
				document.AppendChild(header);
				
				XmlNode coreNode = document.CreateNode(XmlNodeType.Element, _GORE_TAG, string.Empty);
				header.AppendChild(coreNode);
				
				XmlNode backupTasksNode = document.CreateNode(XmlNodeType.Element, "BackupTasks", string.Empty);
				header.AppendChild(backupTasksNode);
				
				XmlNode performanceNode = document.CreateNode(XmlNodeType.Element, _PERFORMANCE_TAG, string.Empty);
				coreNode.AppendChild(performanceNode);
				
				XmlNode securityNode = document.CreateNode(XmlNodeType.Element, _SECURITY_TAG, string.Empty);
				coreNode.AppendChild(securityNode);
				
				XmlNode logsNode = document.CreateNode(XmlNodeType.Element, _LOGS_TAG, string.Empty);
				coreNode.AppendChild(logsNode);
				
				XmlNode scheduleApplicationNode = document.CreateNode(XmlNodeType.Element, "ScheduleApplication", string.Empty);
				coreNode.AppendChild(scheduleApplicationNode);

				XmlNode configuratorApplicationNode = document.CreateNode(XmlNodeType.Element, _CONFIGURATOR_TAG, string.Empty);
				coreNode.AppendChild(configuratorApplicationNode);
				
				addTextNode(document, scheduleApplicationNode, "PuttingOffBackupCpuLoading", options.PuttingOffBackupCpuLoading.ToString());
				
				addTextNode(document, performanceNode, "Parallel", options.Parallel.ToString());

				addTextNode(document, logsNode, "Location", options.LogsFolder);
				
                document.Save(Files.ProfileFile);
            }
            catch (Exception e)
            {
            	System.Diagnostics.Debug.WriteLine(e.ToString());
                throw new OptionsException(
                    string.Format(CultureInfo.CurrentUICulture, "During working with file '{0}' an error occured: {1}. \n\nSettings are not saved", Files.ProfileFile, e.Message));
            }
        }
        
        /// <summary>
        /// Loads settings from an xml document
        /// </summary>
        /// <returns>The options</returns>
        /// <exception cref="OptionsException">Any problems during loading</exception>
        public static ProgramOptions LoadSettings()
        {
        	ProgramOptions options = new ProgramOptions();

            try
            {
                if (File.Exists(Files.ProfileFile))
				{
					XmlDocument document = new XmlDocument();
					document.Load(Files.ProfileFile);
					
					options.Parallel = readNode(document, "/Settings/Core/Performance/Parallel", 1, 99999, Environment.ProcessorCount);

					options.LogsFolder = readNode(document, "/Settings/Core/Logging/Location", Directories.LogsFolder);
					
					options.PuttingOffBackupCpuLoading = (byte)readNode(document, "/Settings/Core/ScheduleApplication/PuttingOffBackupCpuLoading", Constants.MinimumCpuLoading, Constants.MaximumCpuLoading, Constants.DefaultCpuLoading);
                }
            }
            catch (Exception exc)
            {
            	Debug.WriteLine(exc.ToString());
                throw new OptionsException(
                    string.Format(CultureInfo.CurrentUICulture, _fileInaccessibleOrCorrupted, Files.ProfileFile, exc.Message));
            }

            return options;
        }
        
        #endregion
    }
}
