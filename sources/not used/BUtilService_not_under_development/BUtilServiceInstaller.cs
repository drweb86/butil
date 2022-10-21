using System;
using System.ServiceProcess;
using System.Configuration.Install;
using System.ComponentModel;
using BUtil.Common;

namespace BUutilService
{
    [RunInstallerAttribute(true)]
    public sealed class BUtilServiceInstaller : Installer
    {
        private const string _SERVICEDESCRIPTION = "This is a backup scheduling service. It plans time for scheduled backups. Part of a butil project.";

        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;

        public BUtilServiceInstaller()
        {
            // Instantiate installers for process and services.
            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.User;
            _serviceInstaller.StartType = ServiceStartMode.Automatic;

            _processInstaller.Username = Environment.UserDomainName + "\\" + Environment.UserName;
            _serviceInstaller.ServiceName = CopyrightInfo.ServiceName;
            _serviceInstaller.Description = _SERVICEDESCRIPTION;
            _serviceInstaller.DisplayName = CopyrightInfo.ServiceName;

            Installers.Add(_serviceInstaller);
            Installers.Add(_processInstaller);

        }
    }
}
