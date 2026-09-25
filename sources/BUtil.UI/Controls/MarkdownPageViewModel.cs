using BUtil.Core.Legal;
using BUtil.Core.Localization;

namespace BUtil.UI.Controls;

public enum MarkdownPage
{
    Credits,
    ThirdPartyNotices,
}

public class MarkdownPageViewModel : ViewModelBase
{
    public string CloseLabel => Resources.Button_Close;
    public string Markdown { get; }

    public MarkdownPageViewModel(MarkdownPage page)
    {
        Markdown = page == MarkdownPage.Credits
            ? CreditDocuments.Load()
            : ThirdPartyNotices.Load();
        WindowTitle = page == MarkdownPage.Credits
            ? Resources.Menu_Credits
            : Resources.Menu_ThirdPartyNotices;
        IsFullMenuVisible = true;
    }

#pragma warning disable CA1822 // Mark members as static
    public void CloseCommand()
#pragma warning restore CA1822 // Mark members as static
    {
        WindowManager.SwitchView(new TasksViewModel());
    }
}
