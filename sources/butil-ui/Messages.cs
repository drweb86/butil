using BUtil.Core.Localization;
using butil_ui.Views;
using DialogHostAvalonia;
using System.Threading.Tasks;

namespace butil_ui;

public static class Messages
{
    public static async Task ShowInformationBox(string message)
    {
        await DialogHost.Show(new DialogViewModel(Resources.Messages_Header_Information, message));
    }

    public static async Task ShowErrorBox(string message)
    {
        await DialogHost.Show(new DialogViewModel(Resources.Messages_Header_Error, message));
    }

    public static async Task<bool> ShowYesNoDialog(string question)
    {
        var result = await DialogHost.Show(new DialogViewModel(null, question, Resources.Button_OK, Resources.Button_Cancel));
        return (result as string) == "1";
    }
}
