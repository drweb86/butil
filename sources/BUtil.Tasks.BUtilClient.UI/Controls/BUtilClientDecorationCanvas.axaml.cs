using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Tasks.BUtilClient.UI.Controls;

public partial class BUtilClientDecorationCanvas : UserControl
{
    private static readonly Point DotP0 = new(520, 330), DotP1 = new(620, 300), DotP2 = new(700, 350), DotP3 = new(752, 415);

    private const double SweatSpawnX = 235;
    private const double SweatSpawnY = 175;
    private const double SweatEndY = 310;
    private const int SweatCount = 2;

    private readonly Random _random = new();
    private readonly List<Drop> _drops = [];
    private CancellationTokenSource? _cts;

    public BUtilClientDecorationCanvas() => InitializeComponent();

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        BuildScene();
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _cts?.Cancel();
        _cts = null;
        SweatCanvas.Children.Clear();
        _drops.Clear();
    }

    private void BuildScene()
    {
        for (int i = 0; i < SweatCount; i++)
        {
            var shape = new Ellipse
            {
                Width = 10,
                Height = 15,
                Fill = new SolidColorBrush(Color.FromRgb(0x64, 0xB5, 0xF6)),
            };
            var drop = new Drop(shape);
            RespawnDrop(drop, scatter: true);
            _drops.Add(drop);
            SweatCanvas.Children.Add(shape);
        }
    }

    private void RespawnDrop(Drop drop, bool scatter)
    {
        drop.Y = scatter ? SweatSpawnY + _random.NextDouble() * (SweatEndY - SweatSpawnY) : SweatSpawnY;
        drop.X = SweatSpawnX + (_random.NextDouble() - 0.5) * 30;
        drop.VelocityY = 0.6 + _random.NextDouble() * 0.4;
        drop.Life = scatter ? _random.NextDouble() : 0;
    }

    private void UpdateDrops()
    {
        foreach (var drop in _drops)
        {
            drop.VelocityY += 0.05;
            drop.Y += drop.VelocityY;
            drop.Life += 0.02;
            if (drop.Y > SweatEndY)
                RespawnDrop(drop, scatter: false);

            Canvas.SetLeft(drop.Shape, drop.X);
            Canvas.SetTop(drop.Shape, drop.Y);
            drop.Shape.Opacity = Math.Min(1, drop.Life * 3) * 0.85;
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        double dotT = 0;
        double burstLife = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                dotT += 0.014;
                if (dotT > 1)
                {
                    dotT = 0;
                    burstLife = 1;
                }

                var p = Bezier(DotP0, DotP1, DotP2, DotP3, dotT);
                Canvas.SetLeft(UploadDot, p.X - UploadDot.Width / 2);
                Canvas.SetTop(UploadDot, p.Y - UploadDot.Height / 2);
                var edgeFade = Math.Min(1, Math.Min(dotT, 1 - dotT) * 8);
                UploadDot.Opacity = 0.3 + 0.7 * edgeFade;

                if (burstLife > 0)
                {
                    burstLife -= 0.06;
                    JamBurst.Opacity = Math.Max(0, burstLife);
                }

                UpdateDrops();

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

    private sealed class Drop(Ellipse shape)
    {
        public Ellipse Shape { get; } = shape;
        public double X;
        public double Y;
        public double VelocityY;
        public double Life;
    }
}
