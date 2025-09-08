#nullable enable
namespace Terminal.Gui.App;

public sealed class ApplicationOptions
{
    public bool UseAlternateScreenBuffer { get; init; } = false;
    public MouseTrackingMode MouseTracking { get; init; } = MouseTrackingMode.Basic;
    public bool RestoreConsoleOnExit { get; init; } = true;
    public bool ClearOnInit { get; init; } = false;
    public bool EnableBracketedPaste { get; init; } = true;
    public bool EnableFrameBufferFlush { get; init; } = false;
}

public enum MouseTrackingMode
{
    None,
    Basic,
    AnyEvent,
    Sgr,
}
