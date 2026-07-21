using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.UI.Controls;

public partial class HackerDecorationCanvas : UserControl
{
    private static readonly string[] Bits = ["0", "1"];
    private static readonly FontFamily CodeFont = new("Consolas,Menlo,DejaVu Sans Mono,monospace");
    private static readonly SolidColorBrush HeadBrush = new(Color.FromRgb(0xE8, 0xFF, 0xF4));

    private static readonly Color[] RainPalette =
    [
        Color.FromRgb(0x00, 0xFF, 0x41), // matrix green
        Color.FromRgb(0x00, 0xE5, 0xFF), // cyan
        Color.FromRgb(0xFF, 0x2F, 0xD6), // magenta
        Color.FromRgb(0xFF, 0xD6, 0x00), // gold
        Color.FromRgb(0x9D, 0x7C, 0xFF), // violet
    ];

    private static readonly Color[] MotePalette =
    [
        Color.FromRgb(0x00, 0xE5, 0xFF),
        Color.FromRgb(0x00, 0xFF, 0x41),
        Color.FromRgb(0xFF, 0xD6, 0x00),
    ];

    private const double SceneWidth = 240;
    private const double SceneHeight = 184;
    private const int ColumnCount = 12;
    private const int TrailLength = 9;
    private const double CharHeight = 12;
    private const int MoteCount = 8;

    private readonly Random _random = new();
    private readonly List<RainColumn> _columns = [];
    private readonly List<Mote> _motes = [];
    private CancellationTokenSource? _cts;

    public HackerDecorationCanvas() => InitializeComponent();

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
        RainCanvas.Children.Clear();
        ParticlesCanvas.Children.Clear();
        _columns.Clear();
        _motes.Clear();
    }

    private void BuildScene()
    {
        for (int i = 0; i < ColumnCount; i++)
        {
            var column = CreateColumn();
            _columns.Add(column);
            foreach (var ch in column.Chars)
                RainCanvas.Children.Add(ch);
        }

        for (int i = 0; i < MoteCount; i++)
        {
            var mote = CreateMote(true);
            _motes.Add(mote);
            ParticlesCanvas.Children.Add(mote.Shape);
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        int frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                UpdateRain();
                UpdateMotes();
                UpdateGlow(frame);
                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private string RandomBit() => Bits[_random.Next(2)];

    private RainColumn CreateColumn()
    {
        var trailBrush = new SolidColorBrush(Colors.White);
        var headGlow = new DropShadowEffect { BlurRadius = 8, OffsetX = 0, OffsetY = 0, Opacity = 0.9 };
        var chars = new TextBlock[TrailLength];
        for (int i = 0; i < TrailLength; i++)
        {
            bool isHead = i == 0;
            chars[i] = new TextBlock
            {
                FontFamily = CodeFont,
                FontWeight = isHead ? FontWeight.Bold : FontWeight.Normal,
                Foreground = isHead ? HeadBrush : trailBrush,
                Effect = isHead ? headGlow : null,
            };
        }
        var column = new RainColumn(chars, trailBrush, headGlow);
        ResetColumn(column, true);
        return column;
    }

    private void ResetColumn(RainColumn column, bool scatter)
    {
        column.X = 2 + _random.NextDouble() * (SceneWidth - 14);
        column.HeadY = scatter
            ? -_random.NextDouble() * SceneHeight
            : -TrailLength * CharHeight - _random.NextDouble() * 60;
        column.Size = 10 + _random.NextDouble() * 4;
        column.Depth = (column.Size - 10) / 4;
        column.Speed = 0.6 + column.Depth * 1.8 + _random.NextDouble() * 0.6;

        var color = RainPalette[_random.Next(RainPalette.Length)];
        column.TrailBrush.Color = color;
        column.HeadGlow.Color = color;

        for (int i = 0; i < TrailLength; i++)
        {
            var ch = column.Chars[i];
            ch.Text = RandomBit();
            ch.FontSize = column.Size;
            if (i > 0)
                ch.Opacity = Math.Pow(0.8, i) * (0.35 + 0.65 * column.Depth);
        }
    }

    private void UpdateRain()
    {
        foreach (var column in _columns)
        {
            column.HeadY += column.Speed;
            if (column.HeadY - TrailLength * CharHeight > SceneHeight)
                ResetColumn(column, false);

            for (int i = 0; i < TrailLength; i++)
            {
                var ch = column.Chars[i];
                Canvas.SetLeft(ch, column.X);
                Canvas.SetTop(ch, column.HeadY - i * CharHeight);
                if (_random.NextDouble() < 0.05)
                    ch.Text = RandomBit();
            }
        }
    }

    private Mote CreateMote(bool scatter)
    {
        var size = 1.6 + _random.NextDouble() * 2.2;
        var shape = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(MotePalette[_random.Next(MotePalette.Length)]),
        };
        var mote = new Mote(shape);
        RespawnMote(mote, scatter);
        return mote;
    }

    private void RespawnMote(Mote mote, bool scatter)
    {
        mote.X = 10 + _random.NextDouble() * (SceneWidth - 20);
        mote.Y = scatter
            ? _random.NextDouble() * SceneHeight
            : SceneHeight * 0.55 + _random.NextDouble() * SceneHeight * 0.4;
        mote.Speed = 0.25 + _random.NextDouble() * 0.5;
        mote.Drift = (_random.NextDouble() - 0.5) * 0.3;
        mote.Life = 1.0;
        mote.Decay = 0.004 + _random.NextDouble() * 0.008;
    }

    private void UpdateMotes()
    {
        foreach (var mote in _motes)
        {
            mote.Y -= mote.Speed;
            mote.X += mote.Drift;
            mote.Life -= mote.Decay;
            if (mote.Life <= 0 || mote.Y < 8)
                RespawnMote(mote, false);

            Canvas.SetLeft(mote.Shape, mote.X);
            Canvas.SetTop(mote.Shape, mote.Y);
            mote.Shape.Opacity = mote.Life * 0.85;
        }
    }

    private void UpdateGlow(int frame)
    {
        double pulse = 0.5 + 0.5 * Math.Sin(frame * 0.06);
        GlowEllipse.Opacity = 0.5 + 0.35 * pulse;
    }

    private sealed class RainColumn(TextBlock[] chars, SolidColorBrush trailBrush, DropShadowEffect headGlow)
    {
        public TextBlock[] Chars { get; } = chars;
        public SolidColorBrush TrailBrush { get; } = trailBrush;
        public DropShadowEffect HeadGlow { get; } = headGlow;
        public double X;
        public double HeadY;
        public double Speed;
        public double Size;
        public double Depth;
    }

    private sealed class Mote(Ellipse shape)
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
