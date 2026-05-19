namespace BUtil.Interop.UI.Tasks;

public sealed record TaskUICreateMenuRegistration(
    Type ModelType,
    string Header,
    string Group,
    int PreferredOrder);
