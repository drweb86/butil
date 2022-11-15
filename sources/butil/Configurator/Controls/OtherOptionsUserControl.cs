using System;
using BUtil.Core.Options;
using BUtil.Core;
using BUtil.Configurator.Localization;

namespace BUtil.Configurator.Controls
{
	internal sealed partial class OtherOptionsUserControl : BUtil.Core.PL.BackUserControl
	{
		ProgramOptions _programOptions;
		
		public OtherOptionsUserControl()
		{
			InitializeComponent();
			_parallelNumericUpDown.Minimum = 1;
			_parallelNumericUpDown.Maximum = Environment.ProcessorCount;
			_parallelNumericUpDown.Value = Environment.ProcessorCount;
		}
		
		#region Overrides
		
		public override void ApplyLocalization() 
		{
            SetHintForControl(cpuLoadingNumericUpDown, Resources.DefaultIs60);
            putOffBackupTillLabel.Text = Resources.PutOffMakingBackupTillProcessorSLoadingWillBeLessThen;
            _parallelLabel.Text = Resources.ParallelDegree;
		}
	
		public override void SetOptionsToUi(object settings)
		{
            _programOptions = (ProgramOptions)settings;
			cpuLoadingNumericUpDown.Value = _programOptions.PuttingOffBackupCpuLoading;
			_parallelNumericUpDown.Value = _programOptions.Parallel;
		}

		public override void GetOptionsFromUi()
		{
			_programOptions.PuttingOffBackupCpuLoading = Convert.ToByte(cpuLoadingNumericUpDown.Value);
			_programOptions.Parallel = Convert.ToInt32(_parallelNumericUpDown.Value);
		}

		#endregion
	}
}
