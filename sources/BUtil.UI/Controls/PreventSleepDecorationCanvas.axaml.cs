using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class PreventSleepDecorationCanvas : UserControl
{
    private readonly Random _random = new();
    private CancellationTokenSource? _cts;

    public PreventSleepDecorationCanvas() => InitializeComponent();

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _cts?.Cancel();
        _cts = null;
    }

    private async Task RunAsync(CancellationToken ct)
    {
        Path[] stars = [Star1, Star2, Star3, Star4, Star5, Star6, Star7];
        double[] centerX = [40, 95, 60, 135, 30, 110, 165];
        double[] centerY = [40, 25, 110, 70, 170, 160, 165];
        var scales = new ScaleTransform[stars.Length];
        var periods = new double[stars.Length];
        var offsets = new double[stars.Length];

        for (int i = 0; i < stars.Length; i++)
        {
            scales[i] = new ScaleTransform(1, 1);
            stars[i].RenderTransform = new TransformGroup
            {
                Children =
                [
                    new TranslateTransform(-centerX[i], -centerY[i]),
                    scales[i],
                    new TranslateTransform(centerX[i], centerY[i]),
                ],
            };
            periods[i] = 90 + _random.NextDouble() * 90;
            offsets[i] = _random.NextDouble();
        }

        long frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    var cycle = ((frame / periods[i]) + offsets[i]) % 1.0;
                    var fade = cycle < 0.5 ? cycle * 2 : (1 - cycle) * 2;
                    stars[i].Opacity = fade;
                    var scale = 0.85 + 0.3 * fade;
                    scales[i].ScaleX = scale;
                    scales[i].ScaleY = scale;
                }

                MoonGlow.Opacity = 0.25 + 0.12 * Math.Sin(frame * 0.02);

                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            foreach (var star in stars)
            {
                star.Opacity = 1;
                star.RenderTransform = null;
            }
            MoonGlow.Opacity = 0.3;
        }
    }
}
