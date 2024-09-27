using BUtil.Core.Localization;

namespace butil_ui.Views;

public class DialogViewModel(string? header, string message, string? button1 = null, string? button2 = null)
{
    public string? Header { get; } = header;
    public string Message { get; } = message;
    public string? Button1Text { get; } = button1 ?? Resources.Button_OK;
    public string Button1Arg { get; } = "1";
    public string? Button2Text { get; } = button2;
    public string Button2Arg { get; } = "2";
    public bool Button2Visible { get; } = button2 != null;
}
