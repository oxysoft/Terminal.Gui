#nullable enable
using Terminal.Gui.Diagnostics;

namespace Terminal.Gui.Views;

/// <summary>Small overlay displaying per-frame diagnostics (ms and output chars).</summary>
public sealed class PerfOverlayView : View
{
    private readonly int _updateMs;

    public PerfOverlayView(int updateMs = 250)
    {
        _updateMs = Math.Max(50, updateMs);
        CanFocus = false;
        Width = 24; Height = 1;
        X = Pos.AnchorEnd(Width);
        Y = 0;
        Application.AddTimeout(TimeSpan.FromMilliseconds(_updateMs), () => { SetNeedsDraw(); return true; });
    }

    protected override bool OnDrawingContent()
    {
        string text = $"{PerfMetrics.LastFrameMs,5:F1} ms | {PerfMetrics.LastOutChars,6} ch";
        var display = text.Length > Viewport.Width ? text[..Viewport.Width] : text.PadRight(Viewport.Width);
        SetAttribute(GetAttributeForRole(VisualRole.HotFocus));
        Move(0, 0);
        Application.Driver?.AddStr(display);
        return true;
    }
}

