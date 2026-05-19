namespace BUtil.Interop.Tasks.UI;

internal sealed record TaskUICreateMenuRegistration(
    Type ModelType,
    string Header,
    string Group,
    int PreferredOrder);
