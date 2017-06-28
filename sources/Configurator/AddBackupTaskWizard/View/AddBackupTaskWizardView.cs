using System;
using System.Collections.Generic;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Controls;
using BUtil.Core.Options;
using BUtil.Core.PL;
using BULocalization;

namespace BUtil.Configurator.AddBackupTaskWizard.View
{
    class AddBackupTaskWizardView
    {
        #region Properties

        public PageInfo CurrentPage 
        { 
            get { return _steps[_step]; }
        }

        public bool CanGoNextPage
        {
            get { return _step < (_steps.Count-1); }
        }

        public bool CanGoPreviousPage
        {
            get { return _step > 0; }
        }

        public BackupTask Task { get; private set; }

        #endregion

        #region Constructors

        public AddBackupTaskWizardView(ProgramOptions options)
        {
            _options = options;
            Task = ProgramOptionsManager.GetDefaultBackupTask(Translation.Current[622]);
            _steps.Add(new PageInfo(Translation.Current[623], Translation.Current[624], RegisterControl(BackupTaskViewsEnum.Name, new TaskNameUserControl()), Icons.BackupTask48x48));
            _steps.Add(new PageInfo(Translation.Current[72], Translation.Current[625], RegisterControl(BackupTaskViewsEnum.SourceItems, new SourceItemsUserControl()), Icons.SourceItems48x48));
            _steps.Add(new PageInfo(Translation.Current[79], Translation.Current[626], RegisterControl(BackupTaskViewsEnum.Storages, new StoragesUserControl()), Icons.Storages48x48));

            if (Program.SchedulerInstalled && !options.DontNeedScheduler)
            {
                _steps.Add(new PageInfo(Translation.Current[123], Translation.Current[627], RegisterControl(BackupTaskViewsEnum.Scheduler, new SchedulerUserControl()), Icons.Schedule48x48));
            }
            _steps.Add(new PageInfo(Translation.Current[83], Translation.Current[628], RegisterControl(BackupTaskViewsEnum.Encryption, new EncryptionUserControl()), Icons.Password48x48));
            _steps.Add(new PageInfo(Translation.Current[96], Translation.Current[629], RegisterControl(BackupTaskViewsEnum.OtherOptions, new TaskOtherOptionsUserControl()), Icons.OtherSettings48x48));

            _step = 0;
        }

        #endregion

        #region Public Methods

        public void GetOptions()
        {
            foreach (var page in _steps)
            {
                if (page.ControlToShow != null)
                {
                    page.ControlToShow.GetOptionsFromUi();
                }
            }
        }

        public void Configure()
        {
            _controls[BackupTaskViewsEnum.Name].SetOptionsToUi(Task);
            _controls[BackupTaskViewsEnum.Storages].SetOptionsToUi(Task);
            _controls[BackupTaskViewsEnum.SourceItems].SetOptionsToUi(Task);
            _controls[BackupTaskViewsEnum.Scheduler].SetOptionsToUi(Task);
            _controls[BackupTaskViewsEnum.Encryption].SetOptionsToUi(new object[] { _options, Task });
            _controls[BackupTaskViewsEnum.OtherOptions].SetOptionsToUi(new object[] { Task });
            ((EncryptionUserControl)_controls[BackupTaskViewsEnum.Encryption]).DontCareAboutPasswordLength = _options.DontCareAboutPasswordLength;
            ((StoragesUserControl)_controls[BackupTaskViewsEnum.Storages]).TurnInternetAndLocalNetworkFunctions(!_options.HaveNoNetworkAndInternet);
        }

        public void ApplyLocalization()
        {
            foreach (var page in _steps)
            {
                if (page.ControlToShow != null)
                {
                    page.ControlToShow.ApplyLocalization();
                }
            }
        }

        public void GoNext()
        {
            if (!CanGoNextPage)
            {
                throw new InvalidOperationException();
            }

            _step++;
        }

        public void GoPrevious()
        {
            if (!CanGoPreviousPage)
            {
                throw new InvalidOperationException();
            }

            _step--;
        }

        public BackUserControl RegisterControl(BackupTaskViewsEnum view, BackUserControl control)
        {
            _controls.Add(view, control);
            return control;
        }

        #endregion

        #region Fields

        readonly List<PageInfo> _steps = new List<PageInfo>();
        readonly Dictionary<BackupTaskViewsEnum, BackUserControl> _controls = new Dictionary<BackupTaskViewsEnum, BackUserControl>();
        int _step;
        readonly ProgramOptions _options;

        #endregion
    }
}
