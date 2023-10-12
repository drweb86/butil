using BUtil.Core.BackupModels;
using BUtil.Core.Localization;
using BUtil.Core.Logs;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace BUtil.Core.ConfigurationFileModels.V2
{
    public static class TaskV2Validator
    {
        public static bool TryValidate(TaskV2 task, [NotNullWhen(false)] out string? error)
        {
            if (string.IsNullOrWhiteSpace(task.Name) || ContainsIllegalChars(task.Name))
            {
                error = Resources.Name_Field_Validation;
                return false;
            }

            if (!TaskModelStrategyFactory.TryVerify(new StubLog(), task.Model, out error))
                return false;

            error = null;
            return true;
        }

        private static bool ContainsIllegalChars(string text)
        {
            var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            return invalidChars.Any(ch => text.Contains(ch))
                || text.Contains("..");
        }
    }
}
