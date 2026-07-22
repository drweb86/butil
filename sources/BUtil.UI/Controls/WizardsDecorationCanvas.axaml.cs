using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class WizardsDecorationCanvas : UserControl
{
    private static readonly Color[] BubblePalette =
    [
        Color.FromRgb(0x7C, 0xFC, 0x00), // lawn green
        Color.FromRgb(0x39, 0xFF, 0x14), // neon green
        Color.FromRgb(0xAD, 0xFF, 0x2F), // green yellow
        Color.FromRgb(0x00, 0xE5, 0x77), // emerald
    ];

    private static readonly Color[] SparkPalette =
    [
        Color.FromRgb(0xFF, 0xD6, 0x00), // gold
        Color.FromRgb(0xFF, 0x6D, 0x00), // blaze orange
        Color.FromRgb(0xFF, 0xA9, 0x40), // warm amber
    ];

    // Potion surface and fire positions in control coordinates (image is 480x305 starting at top 63).
    private const double PotionCenterX = 265;
    private const double PotionSurfaceY = 248;
    private const double PotionTopY = 120;
    private const double FireCenterX = 250;
    private const double FireBaseY = 355;
    private const double FireTopY = 290;
    private const int BubbleCount = 10;
    private const int SparkCount = 8;

    private readonly Random _random = new();
    private readonly List<Bubble> _bubbles = [];
    private readonly List<Spark> _sparks = [];
    private CancellationTokenSource? _cts;

    public WizardsDecorationCanvas() => InitializeComponent();

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
        ParticlesCanvas.Children.Clear();
        _bubbles.Clear();
        _sparks.Clear();
    }

    private void BuildScene()
    {
        for (int i = 0; i < BubbleCount; i++)
        {
            var bubble = CreateBubble(true);
            _bubbles.Add(bubble);
            ParticlesCanvas.Children.Add(bubble.Shape);
        }

        for (int i = 0; i < SparkCount; i++)
        {
            var spark = CreateSpark(true);
            _sparks.Add(spark);
            ParticlesCanvas.Children.Add(spark.Shape);
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        int frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                UpdateBubbles();
                UpdateSparks();
                UpdateGlow(frame);
                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private Bubble CreateBubble(bool scatter)
    {
        var size = 2.5 + _random.NextDouble() * 4;
        var shape = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(BubblePalette[_random.Next(BubblePalette.Length)]) { Opacity = 0.75 },
            Stroke = new SolidColorBrush(Colors.White) { Opacity = 0.35 },
            StrokeThickness = 0.8,
        };
        var bubble = new Bubble(shape);
        RespawnBubble(bubble, scatter);
        return bubble;
    }

    private void RespawnBubble(Bubble bubble, bool scatter)
    {
        bubble.X = PotionCenterX - 70 + _random.NextDouble() * 140;
        bubble.Y = scatter
            ? PotionTopY + _random.NextDouble() * (PotionSurfaceY - PotionTopY)
            : PotionSurfaceY - _random.NextDouble() * 12;
        bubble.Speed = 0.25 + _random.NextDouble() * 0.5;
        bubble.Phase = _random.NextDouble() * Math.PI * 2;
        bubble.Life = 1.0;
        bubble.Decay = 0.003 + _random.NextDouble() * 0.007;
    }

    private void UpdateBubbles()
    {
        foreach (var bubble in _bubbles)
        {
            bubble.Y -= bubble.Speed;
            bubble.Phase += 0.09;
            bubble.X += Math.Sin(bubble.Phase) * 0.35;
            bubble.Life -= bubble.Decay;
            if (bubble.Life <= 0 || bubble.Y < PotionTopY)
                RespawnBubble(bubble, false);

            Canvas.SetLeft(bubble.Shape, bubble.X);
            Canvas.SetTop(bubble.Shape, bubble.Y);
            bubble.Shape.Opacity = bubble.Life * 0.9;
        }
    }

    private Spark CreateSpark(bool scatter)
    {
        var size = 1.4 + _random.NextDouble() * 2;
        var shape = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(SparkPalette[_random.Next(SparkPalette.Length)]),
        };
        var spark = new Spark(shape);
        RespawnSpark(spark, scatter);
        return spark;
    }

    private void RespawnSpark(Spark spark, bool scatter)
    {
        spark.X = FireCenterX - 60 + _random.NextDouble() * 120;
        spark.Y = scatter
            ? FireTopY + _random.NextDouble() * (FireBaseY - FireTopY)
            : FireBaseY - _random.NextDouble() * 15;
        spark.Speed = 0.5 + _random.NextDouble() * 0.8;
        spark.Drift = (_random.NextDouble() - 0.5) * 0.5;
        spark.Life = 1.0;
        spark.Decay = 0.006 + _random.NextDouble() * 0.012;
    }

    private void UpdateSparks()
    {
        foreach (var spark in _sparks)
        {
            spark.Y -= spark.Speed;
            spark.X += spark.Drift;
            spark.Life -= spark.Decay;
            if (spark.Life <= 0 || spark.Y < FireTopY)
                RespawnSpark(spark, false);

            Canvas.SetLeft(spark.Shape, spark.X);
            Canvas.SetTop(spark.Shape, spark.Y);
            spark.Shape.Opacity = spark.Life * 0.85;
        }
    }

    private void UpdateGlow(int frame)
    {
        double pulse = 0.5 + 0.5 * Math.Sin(frame * 0.06);
        GlowEllipse.Opacity = 0.5 + 0.35 * pulse;
    }

    private sealed class Bubble(Ellipse shape)
    {
        public Ellipse Shape { get; } = shape;
        public double X;
        public double Y;
        public double Speed;
        public double Phase;
        public double Life;
        public double Decay;
    }

    private sealed class Spark(Ellipse shape)
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
