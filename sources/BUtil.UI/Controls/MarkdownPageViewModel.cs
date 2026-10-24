using BUtil.Core.Legal;
using BUtil.Core.Localization;
using System;

namespace BUtil.UI.Controls;

public class MarkdownPageViewModel : ViewModelBase
{
    public string CloseLabel => Resources.Button_Close;
    public string Markdown { get; }

    public MarkdownPageViewModel()
    {
        Markdown = CreditDocuments.Load().TrimEnd() + Environment.NewLine + Environment.NewLine + ThirdPartyNotices.Load();
        WindowTitle = Resources.Menu_About;
        IsFullMenuVisible = true;
    }

#pragma warning disable CA1822 // Mark members as static
    public void CloseCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TasksViewModel());
    }
}
