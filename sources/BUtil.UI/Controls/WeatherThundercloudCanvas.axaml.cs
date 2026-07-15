using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class WeatherThundercloudCanvas : UserControl
{
    private InputElement? _subscribedParent;
    private CancellationTokenSource? _cts;

    private static readonly double[] RainInitialY = [40.0, 52.0, 44.0, 60.0, 47.0];
    private const double RainMinY = 36.0;
    private const double RainMaxY = 78.0;
    private const double RainSpeed = 1.2;

    public WeatherThundercloudCanvas() => InitializeComponent();

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
        Line[] drops = [Rain1, Rain2, Rain3, Rain4, Rain5];
        double[] y = [.. RainInitialY];
        int frame = 0;

        try
        {
            while (!ct.IsCancellationRequested)
            {
                // Rain drops fall and wrap back to top
                for (int i = 0; i < drops.Length; i++)
                {
                    y[i] += RainSpeed;
                    if (y[i] > RainMaxY)
                        y[i] = RainMinY + (y[i] - RainMaxY);
                    Canvas.SetTop(drops[i], y[i]);
                }

                // Lightning double-flash every ~2.8 s (175 frames × 16 ms)
                int f = frame % 175;
                if      (f == 0)   Lightning.Opacity = 1.0;
                else if (f == 6)   Lightning.Opacity = 0.15;
                else if (f == 12)  Lightning.Opacity = 1.0;
                else if (f == 19)  Lightning.Opacity = 0.15;

                frame++;
                await Task.Delay(16, ct);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            Lightning.Opacity = 1.0;
            for (int i = 0; i < drops.Length; i++)
                Canvas.SetTop(drops[i], RainInitialY[i]);
        }
    }

    private void StopAnimation()
    {
        _cts?.Cancel();
        _cts = null;
    }
}
