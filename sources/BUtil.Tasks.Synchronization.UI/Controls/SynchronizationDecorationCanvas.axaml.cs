using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Tasks.Synchronization.UI.Controls;

public partial class SynchronizationDecorationCanvas : UserControl
{
    private static readonly Point DotP0 = new(890, 380), DotP1 = new(830, 230), DotP2 = new(760, 230), DotP3 = new(705, 390);

    private const double DripStartY = 655;
    private const double DripEndY = 745;
    private const double DripX = 722;

    private CancellationTokenSource? _cts;

    public SynchronizationDecorationCanvas() => InitializeComponent();

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
        var sparkleTransform = new ScaleTransform(1, 1);
        BrushSparkle.RenderTransform = sparkleTransform;
        BrushSparkle.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);

        double dotT = 0;
        double flashLife = 0;
        double dripY = DripStartY;
        double dripLife = 0;
        int frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                dotT += 0.01;
                if (dotT > 1)
                {
                    dotT = 0;
                    flashLife = 1;
                }

                var p = Bezier(DotP0, DotP1, DotP2, DotP3, dotT);
                Canvas.SetLeft(SyncDot, p.X - SyncDot.Width / 2);
                Canvas.SetTop(SyncDot, p.Y - SyncDot.Height / 2);
                var edgeFade = Math.Min(1, Math.Min(dotT, 1 - dotT) * 8);
                SyncDot.Opacity = 0.3 + 0.7 * edgeFade;

                if (flashLife > 0)
                {
                    flashLife -= 0.045;
                    ArrivalFlash.Opacity = Math.Max(0, flashLife);
                }

                var sparkleScale = 0.7 + 0.5 * Math.Max(0, Math.Sin(frame * 0.12));
                sparkleTransform.ScaleX = sparkleTransform.ScaleY = sparkleScale;
                BrushSparkle.Opacity = 0.4 + 0.6 * Math.Max(0, Math.Sin(frame * 0.12));

                dripY += 0.6;
                dripLife += 0.01;
                if (dripY > DripEndY)
                {
                    dripY = DripStartY;
                    dripLife = 0;
                }
                Canvas.SetLeft(PaintDrip, DripX);
                Canvas.SetTop(PaintDrip, dripY);
                PaintDrip.Opacity = Math.Clamp(1 - dripLife, 0, 1) * 0.85;

                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private static Point Bezier(Point p0, Point p1, Point p2, Point p3, double t)
    {
        var mt = 1 - t;
        var x = mt * mt * mt * p0.X + 3 * mt * mt * t * p1.X + 3 * mt * t * t * p2.X + t * t * t * p3.X;
        var y = mt * mt * mt * p0.Y + 3 * mt * mt * t * p1.Y + 3 * mt * t * t * p2.Y + t * t * t * p3.Y;
        return new Point(x, y);
    }
}
