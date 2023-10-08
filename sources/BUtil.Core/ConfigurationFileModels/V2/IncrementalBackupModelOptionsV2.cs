﻿
using System;
using System.Collections.Generic;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public class IncrementalBackupModelOptionsV2 : ITaskModelOptionsV2
    {
        public IncrementalBackupModelOptionsV2()
        {
            Items = new();
            FileExcludePatterns = new List<string>();
        }
        public IStorageSettingsV2 To { get; set; }
        public List<SourceItemV2> Items { get; set; }
        public List<string> FileExcludePatterns { get; set; }
        public string Password { get; set; }

        public static IncrementalBackupModelOptionsV2 CreateDefault()
        {
            return new IncrementalBackupModelOptionsV2
            {
                Items = new List<SourceItemV2> {
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) , true),
                    new SourceItemV2(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) , true)}
            };
        }
    }
}
