#nullable enable
using System.Collections.ObjectModel;

namespace Terminal.Gui.Views;

/// <summary>
/// A lightweight, virtualized log view that renders only the visible lines.
/// Optimized for large, append-only (or mostly append) scenarios.
/// </summary>
public sealed class VirtualLogView : View
{
    private IReadOnlyList<string> _items = Array.Empty<string>();
    private int _firstIndex = 0;
    private bool _autoScroll = true;

    public VirtualLogView()
    {
        CanFocus = true;
    }

    /// <summary>Sets the source of lines to render.</summary>
    public void SetSource(IReadOnlyList<string> items)
    {
        _items = items ?? Array.Empty<string>();
        if (_autoScroll) ScrollToEnd();
        SetNeedsDraw();
    }

    /// <summary>Appends a new line and keeps bottom in view if AutoScroll is true.</summary>
    public void Append(string line)
    {
        if (_items is List<string> list) { list.Add(line); }
        else if (_items is ObservableCollection<string> oc) { oc.Add(line); }
        else
        {
            // fallback: copy on write
            var tmp = _items.ToList(); tmp.Add(line); _items = tmp;
        }
        if (_autoScroll) ScrollToEnd();
        SetNeedsDraw();
    }

    /// <summary>Scrolls to show the last page.</summary>
    public void ScrollToEnd()
    {
        int visible = Math.Max(0, Viewport.Height);
        _firstIndex = Math.Max(0, _items.Count - visible);
    }

    public bool AutoScroll
    {
        get => _autoScroll; set { _autoScroll = value; if (value) ScrollToEnd(); SetNeedsDraw(); }
    }

    protected override bool OnDrawingContent()
    {
        SetAttribute(GetAttributeForRole(VisualRole.Normal));
        int width = Viewport.Width;
        int height = Viewport.Height;
        for (int row = 0; row < height; row++)
        {
            int idx = _firstIndex + row;
            Move(0, row);
            if (idx >= 0 && idx < _items.Count)
            {
                var text = _items[idx] ?? string.Empty;
                var line = text.Replace("\r", string.Empty);
                if (line.Length > width) line = line.AsSpan(0, Math.Max(0, width)).ToString();
                Application.Driver?.AddStr(line.PadRight(width));
            }
            else
            {
                Application.Driver?.AddStr(new string(' ', Math.Max(0, width)));
            }
        }
        return true;
    }

    // Keyboard handling can be added via KeyBindings externally if needed.

    private void Line(int delta)
    {
        _autoScroll = false;
        _firstIndex = Math.Max(0, Math.Min(_firstIndex + delta, Math.Max(0, _items.Count - Viewport.Height)));
        SetNeedsDraw();
    }

    private void Page(int pages)
    {
        _autoScroll = false;
        int delta = pages * Math.Max(1, Viewport.Height - 1);
        _firstIndex = Math.Max(0, Math.Min(_firstIndex + delta, Math.Max(0, _items.Count - Viewport.Height)));
        SetNeedsDraw();
    }
}
