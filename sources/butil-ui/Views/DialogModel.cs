using BUtil.Core.Localization;

namespace butil_ui.Views;

public class DialogViewModel
{
    public DialogViewModel(string header, string message, string? button1 = null, string? button2 = null)
    {
        Header = header;
        Message = message;
        Button1Text = button1 ?? Resources.Button_OK;
        Button2Visible = button2 != null;
        Button2Text = button2;
    }
    public string Header { get; }
    public string Message { get; }

    public string? Button1Text { get; }
    public string Button1Arg { get; } = "1";

    public string? Button2Text { get; }
    public string Button2Arg { get; } = "2";
    public bool Button2Visible { get; } = false;
}
