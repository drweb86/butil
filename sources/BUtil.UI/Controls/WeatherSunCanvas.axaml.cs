using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class WeatherSunCanvas : UserControl
{
    private InputElement? _subscribedParent;
    private CancellationTokenSource? _cts;

    public WeatherSunCanvas() => InitializeComponent();

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var card = this.FindAncestorOfType<UserControl>();
        if (card is InputElement parent)
        {
            _subscribedParent = parent;
            parent.PointerEntered += OnParentPointerEntered;
            parent.PointerExited += OnParentPointerExited;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_subscribedParent != null)
        {
            _subscribedParent.PointerEntered -= OnParentPointerEntered;
            _subscribedParent.PointerExited -= OnParentPointerExited;
            _subscribedParent = null;
        }
        StopAnimation();
    }

    private void OnParentPointerEntered(object? sender, PointerEventArgs e) => StartAnimation();
    private void OnParentPointerExited(object? sender, PointerEventArgs e) => StopAnimation();

    private void StartAnimation()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
    }

    private async Task RunAsync(CancellationToken ct)
    {
        var transform = new RotateTransform(0);
        RenderTransform = transform;
        RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);

        try
        {
            while (!ct.IsCancellationRequested)
            {
                transform.Angle = (transform.Angle + 1.5) % 360;
                await Task.Delay(16, ct);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            RenderTransform = null;
        }
    }

    private void StopAnimation()
    {
        _cts?.Cancel();
        _cts = null;
    }
}
