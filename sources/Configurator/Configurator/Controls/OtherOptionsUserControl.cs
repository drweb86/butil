using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;

using BUtil.Core.Options;
using BUtil.Core.PL;
using BUtil.Core;
using BUtil.Configurator.Controls;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
	/// <summary>
	/// Set of various uncategorized options
	/// </summary>
	internal sealed partial class OtherOptionsUserControl : BUtil.Core.PL.BackUserControl
	{
		#region Fields
		
		ProgramOptions _profileOptions;
		
		#endregion
		
		public OtherOptionsUserControl()
		{
			InitializeComponent();
			
			amountOfStoragesToProcessSynchronouslyNumericUpDown.Minimum = Constants.AmountOfStoragesToProcessSynchronouslyMinimum;
			amountOfStoragesToProcessSynchronouslyNumericUpDown.Maximum = Constants.AmountOfStoragesToProcessSynchronouslyMaximum;
			amountOfStoragesToProcessSynchronouslyNumericUpDown.Value = Constants.AmountOfStoragesToProcessSynchronouslyDefault;
			
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Minimum = 1;
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Maximum = Environment.ProcessorCount;
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Value = Environment.ProcessorCount;
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
			int priorityIndex = priorityComboBox.SelectedIndex;
			priorityComboBox.Items.Clear();
            priorityComboBox.Items.Add(Resources.Low);
            priorityComboBox.Items.Add(Resources.BelowNormal);
            priorityComboBox.Items.Add(Resources.Normal);
            priorityComboBox.Items.Add(Resources.AboveNormal);
            chooseBackUpPriorityLabel.Text = Resources.ChooseBackupPriority;
            SetHintForControl(priorityComboBox, Resources.TheBestChoiceIsLowInAlmostAllCases);
            SetHintForControl(cpuLoadingNumericUpDown, Resources.DefaultIs60);
            if (priorityIndex < 0) 
            {
            	priorityIndex = (int)System.Threading.ThreadPriority.BelowNormal;
            }
			priorityComboBox.SelectedIndex = priorityIndex;
			priorityComboBox.Text = priorityComboBox.Items[priorityIndex] as string;
			priorityComboBox.SelectedItem = priorityComboBox.Items[priorityIndex];

			priorityComboBox.Update();
            putOffBackupTillLabel.Text = Resources.PutOffMakingBackupTillProcessorSLoadingWillBeLessThen;
            amountOfStoragesToProcessSynchronouslyLabel.Text = Resources.AmountOfStoragesToProcessSynchronously;
            amountOf7ZipProcessesToRunSynchronouslyLabel.Text = Resources.AmountOf7ZipProcessesToRunSynchronously;
		}
	
		public override void SetOptionsToUi(object settings)
		{
            _profileOptions = (ProgramOptions)settings;
			priorityComboBox.SelectedIndex = (int)_profileOptions.Priority;
            
			// performing additional checks for UI
			if (priorityComboBox.SelectedIndex < 0)
			{
				priorityComboBox.SelectedIndex = (int)System.Threading.ThreadPriority.BelowNormal;
			}

			cpuLoadingNumericUpDown.Value = _profileOptions.PuttingOffBackupCpuLoading;
			amountOfStoragesToProcessSynchronouslyNumericUpDown.Value = _profileOptions.AmountOfStoragesToProcessSynchronously;
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Value = _profileOptions.AmountOf7ZipProcessesToProcessSynchronously;
		}

		public override void GetOptionsFromUi()
		{
			_profileOptions.Priority = (System.Threading.ThreadPriority)priorityComboBox.SelectedIndex;
			_profileOptions.PuttingOffBackupCpuLoading = Convert.ToByte(cpuLoadingNumericUpDown.Value);
			_profileOptions.AmountOfStoragesToProcessSynchronously = Convert.ToInt32(amountOfStoragesToProcessSynchronouslyNumericUpDown.Value);
			_profileOptions.AmountOf7ZipProcessesToProcessSynchronously = Convert.ToInt32(amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Value);
		}

		#endregion
	}
}
