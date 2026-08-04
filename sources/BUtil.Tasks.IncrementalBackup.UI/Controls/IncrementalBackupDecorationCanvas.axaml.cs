using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUtil.Tasks.IncrementalBackup.UI.Controls;

public partial class IncrementalBackupDecorationCanvas : UserControl
{
    private const double MugX = 280;
    private const double MugY = 610;
    private const double SteamTopY = 455;
    private const int PuffCount = 3;

    private readonly Random _random = new();
    private readonly List<Puff> _puffs = [];
    private CancellationTokenSource? _cts;

    public IncrementalBackupDecorationCanvas() => InitializeComponent();

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
        SteamCanvas.Children.Clear();
        _puffs.Clear();
    }

    private void BuildScene()
    {
        for (int i = 0; i < PuffCount; i++)
        {
            var puff = CreatePuff();
            RespawnPuff(puff, scatter: true);
            _puffs.Add(puff);
            SteamCanvas.Children.Add(puff.Shape);
        }
    }

    private static Puff CreatePuff()
    {
        var shape = new Ellipse
        {
            Width = 16,
            Height = 22,
            Fill = new SolidColorBrush(Colors.White),
        };
        return new Puff(shape);
    }

    private void RespawnPuff(Puff puff, bool scatter)
    {
        puff.Y = scatter ? MugY - _random.NextDouble() * (MugY - SteamTopY) : MugY;
        puff.X = MugX + (_random.NextDouble() - 0.5) * 8;
        puff.Speed = 0.7 + _random.NextDouble() * 0.5;
        puff.SwayPhase = _random.NextDouble() * Math.PI * 2;
        puff.SwayAmplitude = 6 + _random.NextDouble() * 6;
        puff.Life = scatter ? _random.NextDouble() : 0;
    }

    private void UpdatePuffs()
    {
        foreach (var puff in _puffs)
        {
            puff.Y -= puff.Speed;
            puff.Life += 0.012;
            if (puff.Y < SteamTopY)
                RespawnPuff(puff, scatter: false);

            var sway = Math.Sin(puff.SwayPhase + puff.Life * 3) * puff.SwayAmplitude * puff.Life;
            Canvas.SetLeft(puff.Shape, puff.X + sway);
            Canvas.SetTop(puff.Shape, puff.Y);

            var fadeIn = Math.Min(1, puff.Life * 4);
            var fadeOut = Math.Clamp(1 - puff.Life, 0, 1);
            puff.Shape.Opacity = Math.Min(fadeIn, fadeOut) * 0.5;
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        var ringTransform = new RotateTransform(0);
        SpinnerRing.RenderTransform = ringTransform;
        SpinnerRing.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);

        int frame = 0;
        try
        {
            while (!ct.IsCancellationRequested)
            {
                ringTransform.Angle = (ringTransform.Angle + 2.2) % 360;

                BackupGlow.Opacity = 0.55 + 0.35 * (0.5 + 0.5 * Math.Sin(frame * 0.03));

                var blink = Math.Sin(frame * 0.07);
                BezelLight1.Opacity = blink > 0 ? 1 : 0.25;
                BezelLight2.Opacity = blink > 0 ? 0.25 : 1;

                UpdatePuffs();

                frame++;
                await Task.Delay(33, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private sealed class Puff(Ellipse shape)
    {
        public Ellipse Shape { get; } = shape;
        public double X;
        public double Y;
        public double Speed;
        public double Life;
        public double SwayPhase;
        public double SwayAmplitude;
    }
}
