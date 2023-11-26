using BUtil.Core.BackupModels;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public static class TaskV2Validator
    {
        public static bool TryValidate(TaskV2 task, [NotNullWhen(false)] out string? error)
        {
            if (!new TaskV2StoreService().TryValidate(task.Name, out error))
            {
                return false;
            }

            var memoryLog = new MemoryLog();

            if (!TaskModelStrategyFactory.TryVerify(memoryLog, task.Model, out error))
            {
                error += Environment.NewLine + Environment.NewLine + memoryLog;
                return false;
            }

            error = null;
            return true;
        }
    }
}
