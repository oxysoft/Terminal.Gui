#nullable enable
namespace Terminal.Gui.App;

internal static class ConsoleStateRestorer
{
    public static void Restore()
    {
        var seq = string.Concat(
            "\x1b[m",
            "\x1b[?7h",
            "\x1b[?6l",
            "\x1b[r",
            "\x1b[?69l",
            "\x1b[?25h"
        );
        var mouseOff = string.Concat(
            "\x1b[?1000l",
            "\x1b[?1002l",
            "\x1b[?1003l",
            "\x1b[?1006l",
            "\x1b[?1015l"
        );
        try { Console.Write(seq); } catch { }
        try { Console.Error.Write(seq); } catch { }
        try { Console.Write(mouseOff); } catch { }
        try { Console.Error.Write(mouseOff); } catch { }
    }
}

