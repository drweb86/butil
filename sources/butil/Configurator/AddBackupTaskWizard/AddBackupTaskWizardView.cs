using System;
using System.Collections.Generic;
using BUtil.Configurator.Configurator.Controls;
using BUtil.Configurator.Configurator.Controls.Tasks;
using BUtil.Configurator.Controls;
using BUtil.Configurator.Localization;
using BUtil.Core.Options;
using BUtil.Core.PL;


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

        public ScheduleInfo ScheduleInfo { get; private set; }


        #endregion

        #region Constructors

        public AddBackupTaskWizardView(ProgramOptions options)
        {
            _options = options;
            Task = ProgramOptionsManager.GetDefaultBackupTask(Resources.NewBackupTaskTitle);

            var encryptionUserControl = new EncryptionUserControl(Task);

            _steps.Add(new PageInfo(Resources.WellcomeToBackupTaskCreationWizard, Resources.ThisMasterWillHelpYouToCreateANewTask, RegisterControl(BackupTaskViewsEnum.Name, new TaskNameUserControl()), Icons.BackupTask48x48));
            _steps.Add(new PageInfo(Resources.What, Resources.HereYouMayAddFilesAndFoldersYouWannaToBackup, RegisterControl(BackupTaskViewsEnum.SourceItems, new WhatUserControl()), Icons.SourceItems48x48));
            _steps.Add(new PageInfo(Resources.Where, Resources.HereYouCanSpecifyWhereToSendBackups, RegisterControl(BackupTaskViewsEnum.Storages, new WhereUserControl()), Icons.Storages48x48));
            _steps.Add(new PageInfo(Resources.When, Resources.YouCanSetDaysAndTimesWhenYouWantToStartThisBackupJob, RegisterControl(BackupTaskViewsEnum.Scheduler, new WhenUserControl()), Icons.Schedule48x48));
            _steps.Add(new PageInfo(Resources.How, Resources.HowHint, RegisterControl(BackupTaskViewsEnum.How, new HowUserControl(Task, () => encryptionUserControl.UpdateModel(Task))), Icons.ActionsOfficeChartPieIcon48x48));
            _steps.Add(new PageInfo(Resources.Encryption, Resources.HereYouCanProtectTheBackupWithPasswordThisIsAlsoRequiredForCopyingItOverNetwork, RegisterControl(BackupTaskViewsEnum.Encryption, encryptionUserControl), Icons.Password48x48));
            _steps.Add(new PageInfo(Resources.OtherOptions, Resources.HereYouCanSpecifyOtherSettings, RegisterControl(BackupTaskViewsEnum.OtherOptions, new OtherTaskOptionsUserControl()), Icons.OtherSettings48x48));

            _step = 0;

            ScheduleInfo = new ScheduleInfo();
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
            _controls[BackupTaskViewsEnum.Scheduler].SetOptionsToUi(ScheduleInfo);
            _controls[BackupTaskViewsEnum.OtherOptions].SetOptionsToUi(new object[] { Task });
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
                throw new InvalidOperationException();

            if (!_steps[_step].ControlToShow.ValidateUi())
                return;

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
