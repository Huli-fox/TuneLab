using System.Collections.Generic;
using System.Linq;

namespace TuneLab.UI.Commands;

internal static class ShortcutHelpRegistry
{
    public static IReadOnlyList<ShortcutHelpItem> GetAll()
    {
        return CommandMetadataRegistry
            .GetAll()
            .Where(metadata => metadata.IncludeInShortcutHelp)
            .Select(ToHelpItem)
            .Where(item => item.HasValue)
            .Select(item => item!.Value)
            .OrderBy(item => item.Category)
            .ThenBy(item => item.DisplayName)
            .ToArray();
    }

    static ShortcutHelpItem? ToHelpItem(CommandMetadata metadata)
    {
        if (!ShortcutRegistry.TryGetShortcut(metadata.Id, out var shortcut))
            return null;

        return new ShortcutHelpItem(
            metadata.Id,
            metadata.Category,
            metadata.DisplayName,
            shortcut,
            ShortcutDisplayFormatter.Format(shortcut));
    }
}
