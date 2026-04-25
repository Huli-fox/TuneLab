using System.Collections.Generic;
using Avalonia.Input;
using TuneLab.GUI.Input;

namespace TuneLab.UI.Commands;

internal static class ShortcutDisplayFormatter
{
    public static string Format(Shortcut shortcut)
    {
        List<string> parts = [];

        if ((shortcut.Modifiers & ModifierKeys.Ctrl) != 0)
            parts.Add("Ctrl");
        if ((shortcut.Modifiers & ModifierKeys.Shift) != 0)
            parts.Add("Shift");
        if ((shortcut.Modifiers & ModifierKeys.Alt) != 0)
            parts.Add("Alt");

        parts.Add(FormatKey(shortcut.Key));
        return string.Join("+", parts);
    }

    static string FormatKey(Key key)
    {
        return key switch
        {
            Key.Space => "Space",
            Key.Enter => "Enter",
            Key.Tab => "Tab",
            Key.Delete => "Delete",
            Key.Up => "Up",
            Key.Down => "Down",
            Key.End => "End",
            Key.Decimal => "Num .",
            Key.OemComma => ",",
            Key.D1 => "1",
            Key.D2 => "2",
            Key.D3 => "3",
            Key.D4 => "4",
            Key.D5 => "5",
            Key.D6 => "6",
            _ => key.ToString(),
        };
    }
}
