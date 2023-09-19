﻿namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class FtpsStorageSettingsV2 : IStorageSettingsV2
    {
        public long SingleBackupQuotaGb { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Folder { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}