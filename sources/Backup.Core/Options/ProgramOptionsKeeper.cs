using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

using BUtil.Core.ButilImage;
using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.PL;
using BUtil.Core.Storages;
using BULocalization;

namespace BUtil.Core.Options
{
	/// <summary>
	/// Saves and loads options to xml file
	/// </summary>
    public static class ProgramOptionsKeeper
    {
    	#region Xml Tags
    	
        const string _HEADER_TAG = "Settings";
        const string _PERFORMANCE_TAG = "Performance";
        const string _SECURITY_TAG = "Security";
        const string _GORE_TAG = "Core";
        const string _LOGS_TAG = "Logging";
        const string _HOUR_TAG = "Hour";
        const string _MINUTE_TAG = "Minute";
        const string _TIME_TAG = "Time";
        const string _TARGET_TAG = "Target";
        const string _TYPE_TAG = "Type";
        const string _ASSEMBLY_TAG = "Assembly";
        const string _DAYS_TAG = "Days";
        const string _COMPRESSION_DEGREE_TAG = "CompressionDegree";
        const string _SCHEDULE_TAG = "Schedule";
        const string _IS_FOLDER = "IsFolder";
        const string _WHERE_TAG = "WhereToBackup";
        const string _WHAT_TAG = "WhatToBackup";
        const string _PASSWORD = "SecretPassword";
        const string _NAME = "Name";
        const string _DONT_NEED_SCHEDULER_TAG = "DontNeedScheduler";
        const string _CONFIGURATOR_TAG = "Configurator";
		const string _HAVE_NO_INTERNET_AND_NETWORK_TAG = "HaveNoNetworkAndInternet";
		const string _DONT_CARE_ABOUT_SCHEDULER_STARTUP_TAG = "DontCareAboutSchedulerStartup";
		const string _HIDE_ABOUT_TAB_TAG = "HideAboutTab";
		const string _CHAIN_OF_PROGRAMS_TO_RUN = "ChainOfProgramsToRun";
		const string _BEFORE_BACKUP = "BeforeBackup";
		const string _AFTER_BACKUP = "AfterBackup";
		const string _PROGRAM = "Program";
		const string _ARGUMENTS = "Arguments";
		
        
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
		
        /// <summary>
		/// Stores the settings to an Xml file and encrypts it under local system account
		/// </summary>
		/// <param name="options">The target options object</param>
        /// <exception cref="OptionsException">Any problems with options</exception>
		/// <exception cref="ArgumentException">options argument is null</exception>
        public static void StoreSettings(ProgramOptions options)
        {
			if (options == null)
				throw new ArgumentException("options");

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
				
				addTextNode(document, configuratorApplicationNode, _HAVE_NO_INTERNET_AND_NETWORK_TAG, options.HaveNoNetworkAndInternet.ToString());
				addTextNode(document, configuratorApplicationNode, _DONT_CARE_ABOUT_SCHEDULER_STARTUP_TAG, options.DontCareAboutSchedulerStartup.ToString());
				addTextNode(document, configuratorApplicationNode, _HIDE_ABOUT_TAB_TAG, options.HideAboutTab.ToString());
				
				addTextNode(document, scheduleApplicationNode, "ShowInTray", options.ShowSchedulerInTray.ToString());
				addTextNode(document, scheduleApplicationNode, "PuttingOffBackupCpuLoading", options.PuttingOffBackupCpuLoading.ToString());
				addTextNode(document, scheduleApplicationNode, _DONT_NEED_SCHEDULER_TAG, options.DontNeedScheduler.ToString());
				
				addTextNode(document, performanceNode, "AmountOf7ZipProcessesToProcessSynchronously", options.AmountOf7ZipProcessesToProcessSynchronously.ToString());
				addTextNode(document, performanceNode, "AmountOfStoragesToProcessSynchronously", options.AmountOfStoragesToProcessSynchronously.ToString());
				addTextNode(document, performanceNode, "ProcessingPriority", options.Priority.ToString());

				addTextNode(document, securityNode, "DontCareAboutPasswordLength", options.DontCareAboutPasswordLength.ToString());
                
				addTextNode(document, logsNode, "Level", options.LoggingLevel.ToString());
				addTextNode(document, logsNode, "Location", options.LogsFolder);

				foreach (KeyValuePair<string, BackupTask> pair in options.BackupTasks)
				{
					BackupTask task = pair.Value;
					
					XmlNode backupTaskNode = document.CreateNode(XmlNodeType.Element, "Task", string.Empty);
					backupTasksNode.AppendChild(backupTaskNode);
					
					XmlNode chainsNode = document.CreateNode(XmlNodeType.Element, _CHAIN_OF_PROGRAMS_TO_RUN, string.Empty);
					backupTaskNode.AppendChild(chainsNode);
					
					XmlNode beforeNode = document.CreateNode(XmlNodeType.Element, _BEFORE_BACKUP, string.Empty);
					chainsNode.AppendChild(beforeNode);
					
					XmlNode afterNode = document.CreateNode(XmlNodeType.Element, _AFTER_BACKUP, string.Empty);
					chainsNode.AppendChild(afterNode);
					
					foreach (BackupEventTaskInfo info in task.BeforeBackupTasksChain)
					{
						XmlNode chainNode = document.CreateNode(XmlNodeType.Element, _PROGRAM, string.Empty);
						beforeNode.AppendChild(chainNode);

						addAttributeToNode(document, chainNode, _NAME, info.Program);
						addAttributeToNode(document, chainNode, _ARGUMENTS, info.Arguments);
					}

					foreach (BackupEventTaskInfo info in task.AfterBackupTasksChain)
					{
						XmlNode chainNode = document.CreateNode(XmlNodeType.Element, _PROGRAM, string.Empty);
						afterNode.AppendChild(chainNode);

						addAttributeToNode(document, chainNode, _NAME, info.Program);
						addAttributeToNode(document, chainNode, _ARGUMENTS, info.Arguments);
					}
					
					addAttributeToNode(document, backupTaskNode, _NAME, task.Name);
								
					addTextNode(document, backupTaskNode, _PASSWORD, task.SecretPassword);

					XmlNode whatToBackupNode = document.CreateNode(XmlNodeType.Element, _WHAT_TAG, string.Empty);
					backupTaskNode.AppendChild(whatToBackupNode);
					
					XmlNode whereToBackupNode = document.CreateNode(XmlNodeType.Element, _WHERE_TAG, string.Empty);
					backupTaskNode.AppendChild(whereToBackupNode);
					
					XmlNode scheduleNode = document.CreateNode(XmlNodeType.Element, _SCHEDULE_TAG, string.Empty);
					backupTaskNode.AppendChild(scheduleNode);

					foreach (CompressionItem item in task.FilesFoldersList)
					{
						XmlNode itemTaskNode = document.CreateNode(XmlNodeType.Element, "Item", string.Empty);
						whatToBackupNode.AppendChild(itemTaskNode);
//TODO: this tag is saved only to decrease complexity of porting settings						
//TODO: so when 'Hint' task will be created, this setting must be read
						addAttributeToNode(document, itemTaskNode, _NAME, item.Target);
						addAttributeToNode(document, itemTaskNode, _TARGET_TAG, item.Target);
						addAttributeToNode(document, itemTaskNode, _IS_FOLDER, item.IsFolder.ToString());
						addAttributeToNode(document, itemTaskNode, _COMPRESSION_DEGREE_TAG, item.CompressionDegree.ToString());
					}
					
					foreach (StorageBase storage in task.Storages)
					{
						XmlNode storageNode = document.CreateNode(XmlNodeType.Element, "Storage", string.Empty);
						whereToBackupNode.AppendChild(storageNode);
						
						Dictionary<string, string> values = storage.SaveSettings();
						addAttributeToNode(document, storageNode, _TYPE_TAG, storage.GetType().FullName);
						if (storage.GetType().Assembly != Assembly.GetExecutingAssembly())
						{
							addAttributeToNode(document, storageNode, _ASSEMBLY_TAG, storage.GetType().Assembly.Location);
						}

						foreach (KeyValuePair<string, string> setting in values)
						{
							addTextNode(document, storageNode, setting.Key, setting.Value);
						}
					}
					
					XmlNode zeroHourNode = document.CreateNode(XmlNodeType.Element, _TIME_TAG, string.Empty);
					scheduleNode.AppendChild(zeroHourNode);
					
					XmlNode scheduledDaysNode = document.CreateNode(XmlNodeType.Element, _DAYS_TAG, string.Empty);
					scheduleNode.AppendChild(scheduledDaysNode);
		
					addAttributeToNode(document, zeroHourNode, _HOUR_TAG, task.Hours.ToString());
					addAttributeToNode(document, zeroHourNode, _MINUTE_TAG, task.Minutes.ToString());
					
					foreach(DayOfWeek enumItem in DayOfWeek.GetValues(typeof(DayOfWeek)))
					{
						addAttributeToNode(document, scheduledDaysNode, enumItem.ToString(), task.IsThisDayOfWeekScheduled(enumItem).ToString());
					}
				}
				
                document.Save(Files.ProfileFile);
            }
            catch (Exception e)
            {
            	System.Diagnostics.Debug.WriteLine(e.ToString());
                throw new OptionsException(
                    string.Format(CultureInfo.CurrentCulture, "During working with file '{0}' an error occured: {1}. \n\nSettings are not saved", Files.ProfileFile, e.Message));
            }

            bool notSecure = false;
            string message = string.Empty;
            
            try
            {
                File.Encrypt(Files.ProfileFile);
            }
            catch (PlatformNotSupportedException e)
            {
                notSecure = true;
                message = e.Message;
            }
            catch (NotSupportedException e)
            { 
                notSecure = true;
                message = e.Message;
            }
            catch (IOException e)
            {
                notSecure = true;
                message = e.Message;
            }

            if (notSecure && options.RequiresEncryptionForSafety())
            {
            	// warning about security problem
                Messages.ShowErrorBox(string.Format(CultureInfo.InvariantCulture, Translation.Current[552], Files.ProfileFile, message));
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
					
					options.AmountOf7ZipProcessesToProcessSynchronously = readNode(document, "/Settings/Core/Performance/AmountOf7ZipProcessesToProcessSynchronously", Constants.AmountOf7ZipProcessesToProcessSynchronouslyMinimum, Constants.AmountOf7ZipProcessesToProcessSynchronouslyMaximum, Constants.AmountOf7ZipProcessesToProcessSynchronouslyDefault);
					options.AmountOfStoragesToProcessSynchronously = readNode(document, "/Settings/Core/Performance/AmountOfStoragesToProcessSynchronously", Constants.AmountOfStoragesToProcessSynchronouslyMinimum, Constants.AmountOfStoragesToProcessSynchronouslyMaximum, Constants.AmountOfStoragesToProcessSynchronouslyDefault);
					string priority = readNode(document, "/Settings/Core/Performance/ProcessingPriority", ThreadPriority.BelowNormal.ToString());
					options.Priority =  (ThreadPriority)ThreadPriorityLevel.Parse(typeof(ThreadPriority), priority);

					options.DontCareAboutPasswordLength = readNode(document, "/Settings/Core/Security/DontCareAboutPasswordLength", false);
					
					string logLevel = readNode(document, "/Settings/Core/Logging/Level", LogLevel.Normal.ToString());
					options.LoggingLevel =  (LogLevel)ThreadPriorityLevel.Parse(typeof(LogLevel), logLevel);
					
					options.LogsFolder = readNode(document, "/Settings/Core/Logging/Location", Directories.LogsFolder);
					
					options.ShowSchedulerInTray = readNode(document, "/Settings/Core/ScheduleApplication/ShowInTray", true);
					options.PuttingOffBackupCpuLoading = (byte)readNode(document, "/Settings/Core/ScheduleApplication/PuttingOffBackupCpuLoading", Constants.MinimumCpuLoading, Constants.MaximumCpuLoading, Constants.DefaultCpuLoading);
					options.DontNeedScheduler = readNode(document, "/Settings/Core/ScheduleApplication/" + _DONT_NEED_SCHEDULER_TAG, false);
					
					options.HaveNoNetworkAndInternet = readNode(document, "/Settings/Core/" + _CONFIGURATOR_TAG + "/" + _HAVE_NO_INTERNET_AND_NETWORK_TAG, false);
					options.DontCareAboutSchedulerStartup = readNode(document, "/Settings/Core/" + _CONFIGURATOR_TAG + "/" + _DONT_CARE_ABOUT_SCHEDULER_STARTUP_TAG, false);
					options.HideAboutTab = readNode(document, "/Settings/Core/" + _CONFIGURATOR_TAG + "/" + _HIDE_ABOUT_TAB_TAG, false);
					
					XmlNodeList taskNodes = document.SelectNodes("/Settings/BackupTasks/Task");
					
					foreach (XmlNode taskNode in taskNodes)
					{
						BackupTask task = new BackupTask();
						
						task.Name = taskNode.Attributes[_NAME].Value;
						task.SecretPassword = taskNode[_PASSWORD].InnerText;

						XmlNodeList compressionItemsNodes = taskNode[_WHAT_TAG].ChildNodes;
						XmlNodeList storagesNodes = taskNode[_WHERE_TAG].ChildNodes;
						XmlNodeList beforeNodes = taskNode[_CHAIN_OF_PROGRAMS_TO_RUN][_BEFORE_BACKUP].ChildNodes;
						XmlNodeList afterNodes = taskNode[_CHAIN_OF_PROGRAMS_TO_RUN][_AFTER_BACKUP].ChildNodes;

						foreach (XmlNode nodeItem in beforeNodes)
						{
							BackupEventTaskInfo info = new BackupEventTaskInfo(
								nodeItem.Attributes[_NAME].Value,
								nodeItem.Attributes[_ARGUMENTS].Value);
							task.BeforeBackupTasksChain.Add(info);
						}
						
						foreach (XmlNode nodeItem in afterNodes)
						{
							BackupEventTaskInfo info = new BackupEventTaskInfo(
								nodeItem.Attributes[_NAME].Value,
								nodeItem.Attributes[_ARGUMENTS].Value);
							task.AfterBackupTasksChain.Add(info);
						}
						
						foreach (XmlNode compressionItemNode in compressionItemsNodes)
						{
							CompressionItem item = new CompressionItem(
								compressionItemNode.Attributes[_TARGET_TAG].Value,
								bool.Parse(compressionItemNode.Attributes[_IS_FOLDER].Value),
								(CompressionDegree)CompressionDegree.Parse(typeof(CompressionDegree), compressionItemNode.Attributes[_COMPRESSION_DEGREE_TAG].Value));
							
							task.FilesFoldersList.Add(item);
						}
						
						XmlNode schedule = taskNode[_SCHEDULE_TAG];
						XmlNode zeroHour = schedule[_TIME_TAG];
						XmlNode days = schedule[_DAYS_TAG];
						
						task.Hours = byte.Parse(zeroHour.Attributes[_HOUR_TAG].Value);
						task.Minutes = byte.Parse(zeroHour.Attributes[_MINUTE_TAG].Value);
						
						foreach(DayOfWeek enumItem in DayOfWeek.GetValues(typeof(DayOfWeek)))
						{
							task.SetSchedulingStateOfDay(enumItem, bool.Parse(days.Attributes[enumItem.ToString()].Value));
						}
						
						foreach (XmlNode storageNode in storagesNodes)
						{
							Dictionary<string, string> settings = new Dictionary<string, string>();
							foreach (XmlNode node in storageNode.ChildNodes)
							{
								settings.Add(node.Name, node.InnerText);
							}				
							
							XmlAttribute assemblyAttribute = storageNode.Attributes[_ASSEMBLY_TAG];
							
							// this is done to prevent using different assemblies of a different copies of a program 
							Assembly assembly = (assemblyAttribute != null) ? 
								Assembly.LoadFrom(assemblyAttribute.Value) : 
								Assembly.GetExecutingAssembly();
							string type = storageNode.Attributes[_TYPE_TAG].Value;
							StorageBase storage = (StorageBase)Activator.CreateInstance(assembly.GetType(type), settings);
														
							task.Storages.Add(storage);
						}
						
						options.BackupTasks.Add(task.Name, task);
					}
                }
            }
            catch (Exception exc)
            {
            	Debug.WriteLine(exc.ToString());
                throw new OptionsException(
                    string.Format(CultureInfo.CurrentCulture, _fileInaccessibleOrCorrupted, Files.ProfileFile, exc.Message));
            }

            return options;
        }
        
        #endregion
    }
}
