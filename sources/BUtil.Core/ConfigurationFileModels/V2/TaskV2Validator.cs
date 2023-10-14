using BUtil.Core.BackupModels;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using BUtil.Core.Options;
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

            if (!TaskModelStrategyFactory.TryVerify(new StubLog(), task.Model, out error))
                return false;

            error = null;
            return true;
        }
    }
}
