namespace BUtil.Core.Services
{
    public interface IUiService
    {
        void Blink();
        bool CanExtendClientAreaToDecorationsHint {  get; }
    }
}
