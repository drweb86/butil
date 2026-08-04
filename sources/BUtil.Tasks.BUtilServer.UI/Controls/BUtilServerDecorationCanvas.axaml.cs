using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Tasks.BUtilServer.UI.Controls;

public partial class BUtilServerDecorationCanvas : UserControl
{
    private static readonly Point FlameOrigin = new(345, 180);
    private const double MaxRingRadius = 260;
    private const int RingCount = 3;
    private const int EmberCount = 10;

    private readonly Random _random = new();
    private readonly List<Ring> _rings = [];
    private readonly List<Ember> _embers = [];
    private CancellationTokenSource? _cts;

    public BUtilServerDecorationCanvas() => InitializeComponent();

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
        RingsCanvas.Children.Clear();
        EmbersCanvas.Children.Clear();
        _rings.Clear();
        _embers.Clear();
    }

    private void BuildScene()
    {
        for (int i = 0; i < RingCount; i++)
        {
            var shape = new Ellipse
            {
                Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x8F, 0x00)),
                StrokeThickness = 4,
                Fill = Brushes.Transparent,
            };
            var ring = new Ring(shape) { Life = (double)i / RingCount };
            _rings.Add(ring);
            RingsCanvas.Children.Add(shape);
        }

        for (int i = 0; i < EmberCount; i++)
        {
            var ember = CreateEmber();
            RespawnEmber(ember, scatter: true);
            _embers.Add(ember);
            EmbersCanvas.Children.Add(ember.Shape);
        }
    }

    private Ember CreateEmber()
    {
        var size = 5 + _random.NextDouble() * 6;
        var shape = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(_random.NextDouble() < 0.5
                ? Color.FromRgb(0xFF, 0xB7, 0x4D)
                : Color.FromRgb(0xFF, 0xD5, 0x4F)),
        };
        return new Ember(shape);
    }

    private void RespawnEmber(Ember ember, bool scatter)
    {
        ember.X = 260 + _random.NextDouble() * 300;
        ember.Y = scatter ? -40 + _random.NextDouble() * 320 : 200 + _random.NextDouble() * 40;
        ember.Speed = 0.8 + _random.NextDouble() * 1.4;
        ember.Drift = (_random.NextDouble() - 0.5) * 1.2;
        ember.Life = 1.0;
        ember.Decay = 0.006 + _random.NextDouble() * 0.01;
    }

    private void UpdateEmbers()
    {
        foreach (var ember in _embers)
        {
            ember.Y -= ember.Speed;
            ember.X += ember.Drift;
            ember.Life -= ember.Decay;
            if (ember.Life <= 0 || ember.Y < -60)
                RespawnEmber(ember, scatter: false);

            Canvas.SetLeft(ember.Shape, ember.X);
            Canvas.SetTop(ember.Shape, ember.Y);
            ember.Shape.Opacity = Math.Clamp(ember.Life, 0, 1) * 0.85;
        }
    }

    private void UpdateRings()
    {
        foreach (var ring in _rings)
        {
            ring.Life += 0.01;
            if (ring.Life > 1)
                ring.Life = 0;

            var radius = ring.Life * MaxRingRadius;
            ring.Shape.Width = radius * 2;
            ring.Shape.Height = radius * 2;
            Canvas.SetLeft(ring.Shape, FlameOrigin.X - radius);
            Canvas.SetTop(ring.Shape, FlameOrigin.Y - radius);
            ring.Shape.Opacity = Math.Clamp(0.7 * (1 - ring.Life), 0, 0.7);
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        int frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var flicker = 0.75
                    + 0.15 * Math.Sin(frame * 0.22)
                    + 0.1 * Math.Sin(frame * 0.53 + 1.3)
                    + (_random.NextDouble() - 0.5) * 0.08;
                FlameGlow.Opacity = Math.Clamp(flicker, 0.4, 1);

                UpdateRings();
                UpdateEmbers();

                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private sealed class Ring(Ellipse shape)
    {
        public Ellipse Shape { get; } = shape;
        public double Life;
    }

    private sealed class Ember(Ellipse shape)
    {
        public Ellipse Shape { get; } = shape;
        public double X;
        public double Y;
        public double Speed;
        public double Drift;
        public double Life;
        public double Decay;
    }
}
