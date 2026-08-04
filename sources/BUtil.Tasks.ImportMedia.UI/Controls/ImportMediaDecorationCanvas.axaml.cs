using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Tasks.ImportMedia.UI.Controls;

public partial class ImportMediaDecorationCanvas : UserControl
{
    private static readonly Color[] ChipPalette =
    [
        Color.FromRgb(0x4F, 0xC3, 0xF7), // photo blue
        Color.FromRgb(0xFF, 0xB7, 0x4D), // clip amber
        Color.FromRgb(0xBA, 0x68, 0xC8), // clip violet
        Color.FromRgb(0x81, 0xC7, 0x84), // clip green
    ];

    private const double SceneWidth = 1024;
    private const double SceneHeight = 1024;
    private const int ChipCount = 7;

    private readonly Random _random = new();
    private readonly List<Chip> _chips = [];
    private CancellationTokenSource? _cts;

    public ImportMediaDecorationCanvas() => InitializeComponent();

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
        _chips.Clear();
    }

    private void BuildScene()
    {
        for (int i = 0; i < ChipCount; i++)
        {
            var chip = CreateChip(true);
            _chips.Add(chip);
            ParticlesCanvas.Children.Add(chip.Shape);
        }
    }

    private Chip CreateChip(bool scatter)
    {
        var size = 26 + _random.NextDouble() * 16;
        var shape = new Rectangle
        {
            Width = size,
            Height = size * 0.72,
            RadiusX = 5,
            RadiusY = 5,
            Fill = new SolidColorBrush(ChipPalette[_random.Next(ChipPalette.Length)]),
        };
        var chip = new Chip(shape);
        RespawnChip(chip, scatter);
        return chip;
    }

    private void RespawnChip(Chip chip, bool scatter)
    {
        chip.X = 300 + _random.NextDouble() * 520;
        chip.Y = scatter
            ? 120 + _random.NextDouble() * SceneHeight
            : SceneHeight - 60 - _random.NextDouble() * 120;
        chip.Speed = 1.2 + _random.NextDouble() * 1.6;
        chip.Drift = (_random.NextDouble() - 0.5) * 0.8;
        chip.Life = 1.0;
        chip.Decay = 0.006 + _random.NextDouble() * 0.006;
    }

    private void UpdateChips()
    {
        foreach (var chip in _chips)
        {
            chip.Y -= chip.Speed;
            chip.X += chip.Drift;
            chip.Life -= chip.Decay;
            if (chip.Life <= 0 || chip.Y < 100)
                RespawnChip(chip, false);

            Canvas.SetLeft(chip.Shape, chip.X);
            Canvas.SetTop(chip.Shape, chip.Y);
            chip.Shape.Opacity = Math.Clamp(chip.Life, 0, 1) * 0.9;
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        int frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                UpdateChips();

                GlowEllipse.Opacity = 0.5 + 0.4 * (0.5 + 0.5 * Math.Sin(frame * 0.025));
                RecGlow.Opacity = 0.3 + 0.7 * Math.Max(0, Math.Sin(frame * 0.09));

                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private sealed class Chip(Rectangle shape)
    {
        public Rectangle Shape { get; } = shape;
        public double X;
        public double Y;
        public double Speed;
        public double Drift;
        public double Life;
        public double Decay;
    }
}
