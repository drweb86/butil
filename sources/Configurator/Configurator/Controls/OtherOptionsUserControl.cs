using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using BULocalization;
using BUtil.Core.Options;
using BUtil.Core.PL;
using BUtil.Core;
using BUtil.Configurator.Controls;

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
			
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Minimum = Constants.AmountOf7ZipProcessesToProcessSynchronouslyMinimum;
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Maximum = Constants.AmountOf7ZipProcessesToProcessSynchronouslyMaximum;
			amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Value = Constants.AmountOf7ZipProcessesToProcessSynchronouslyDefault;
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
			int priorityIndex = priorityComboBox.SelectedIndex;
			priorityComboBox.Items.Clear();
            priorityComboBox.Items.Add(Translation.Current[102]);
            priorityComboBox.Items.Add(Translation.Current[101]);
            priorityComboBox.Items.Add(Translation.Current[100]);
            priorityComboBox.Items.Add(Translation.Current[99]);
            chooseBackUpPriorityLabel.Text = Translation.Current[103];
            SetHintForControl(priorityComboBox, Translation.Current[332]);
            SetHintForControl(cpuLoadingNumericUpDown, Translation.Current[104]);
            if (priorityIndex < 0) 
            {
            	priorityIndex = (int)System.Threading.ThreadPriority.BelowNormal;
            }
			priorityComboBox.SelectedIndex = priorityIndex;
			priorityComboBox.Text = priorityComboBox.Items[priorityIndex] as string;
			priorityComboBox.SelectedItem = priorityComboBox.Items[priorityIndex];

			priorityComboBox.Update();
            putOffBackupTillLabel.Text = Translation.Current[105];
            amountOfStoragesToProcessSynchronouslyLabel.Text = Translation.Current[572];
            amountOf7ZipProcessesToRunSynchronouslyLabel.Text = Translation.Current[575];
            minimizeUsageOfSystemResourcesCheckBox.Text = Translation.Current[474];
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
            minimizeUsageOfSystemResourcesCheckBox.Checked = !_profileOptions.ShowSchedulerInTray;
		}

		public override void GetOptionsFromUi()
		{
			_profileOptions.Priority = (System.Threading.ThreadPriority)priorityComboBox.SelectedIndex;
			_profileOptions.PuttingOffBackupCpuLoading = Convert.ToByte(cpuLoadingNumericUpDown.Value);
			_profileOptions.AmountOfStoragesToProcessSynchronously = Convert.ToInt32(amountOfStoragesToProcessSynchronouslyNumericUpDown.Value);
			_profileOptions.AmountOf7ZipProcessesToProcessSynchronously = Convert.ToInt32(amountOf7ZipProcessesToRunSynchronouslyNumericUpDown.Value);
            _profileOptions.ShowSchedulerInTray = !minimizeUsageOfSystemResourcesCheckBox.Checked;
		}

		#endregion
	}
}
