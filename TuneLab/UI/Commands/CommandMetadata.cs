namespace TuneLab.UI.Commands;

internal readonly record struct CommandMetadata(
    CommandId Id,
    CommandCategory Category,
    string DisplayName,
    bool IncludeInShortcutHelp = true);
