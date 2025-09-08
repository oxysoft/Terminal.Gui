#nullable enable
using System.Diagnostics;

namespace Terminal.Gui.Diagnostics;

/// <summary>Collects lightweight per-frame diagnostics (duration, output char count).</summary>
public static class PerfMetrics
{
    private static readonly Stopwatch _sw = new ();

    public static double LastFrameMs { get; private set; }
    public static int LastOutChars { get; private set; }

    public static void FrameBegin()
    {
        LastOutChars = 0;
        _sw.Restart();
    }

    public static void FrameAddOutput(int chars)
    {
        if (chars > 0) LastOutChars += chars;
    }

    public static void FrameEnd()
    {
        _sw.Stop();
        LastFrameMs = _sw.Elapsed.TotalMilliseconds;
    }
}

