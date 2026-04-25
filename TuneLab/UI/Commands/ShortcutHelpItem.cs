namespace TuneLab.UI.Commands;

internal readonly record struct ShortcutHelpItem(
    CommandId Command,
    CommandCategory Category,
    string DisplayName,
    Shortcut Shortcut,
    string ShortcutDisplay);
