using BUtil.Core.Localization;
using butil_ui.Views;
using DialogHostAvalonia;
using System.Threading.Tasks;

namespace butil_ui;

public static class Messages
{
    public static async Task ShowInformationBox(string message)
    {
        var result = await DialogHost.Show(new DialogViewModel(message));

        //await MessageBoxManager
        //    .GetMessageBoxStandard(Resources.Messages_Header_Information, message, ButtonEnum.Ok, Icon.Info)
        //    .ShowAsync();
    }

    public static async Task ShowErrorBox(string message)
    {
        var result = await DialogHost.Show(new DialogViewModel(message));

        //await MessageBoxManager
        //    .GetMessageBoxStandard(Resources.Messages_Header_Error, message, ButtonEnum.Ok, Icon.Error)
        //    .ShowAsync();
    }

    public static async Task<bool> ShowYesNoDialog(string question)
    {
        var result = await DialogHost.Show(new DialogViewModel(question));


        //var response = await MessageBoxManager
        //    .GetMessageBoxStandard(Resources.Messages_Header_Question, question, ButtonEnum.YesNo, Icon.Question)
        //    .ShowAsync();

        //return response == ButtonResult.Yes;
        return false;
    }
}
