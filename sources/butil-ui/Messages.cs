using BUtil.Core.Localization;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Threading.Tasks;

namespace butil_ui
{
    public static class Messages
    {
        public static async Task ShowInformationBox(string message)
        {
            await MessageBoxManager
                .GetMessageBoxStandard(Resources.Messages_Header_Information, message, ButtonEnum.Ok, Icon.Info)
                .ShowAsync();
        }

        public static async Task ShowErrorBox(string message)
        {
            await MessageBoxManager
                .GetMessageBoxStandard(Resources.Messages_Header_Error, message, ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }

        public static async Task<bool> ShowYesNoDialog(string question)
        {
            var response = await MessageBoxManager
                .GetMessageBoxStandard(Resources.Messages_Header_Question, question, ButtonEnum.YesNo, Icon.Question)
                .ShowAsync();

            return response == ButtonResult.Yes;
        }
    }
}
