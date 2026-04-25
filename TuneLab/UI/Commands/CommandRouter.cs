using Avalonia.Input;
using TuneLab.Utils;

namespace TuneLab.UI.Commands;

internal static class CommandRouter
{
    // The unified router only handles command-like shortcuts.
    // Modal editing behavior such as Alt/Shift/Ctrl drag modifiers stays local to each operation.
    public static bool TryHandle(KeyEventArgs e, ICommandContext? context, bool ignoreTextBox = false, bool bubbleToParent = true)
    {
        if (!ignoreTextBox && e.IsHandledByTextBox())
            return false;

        if (!ShortcutRegistry.TryGetCommand(e, out var command))
            return false;

        if (!(bubbleToParent ? TryExecute(context, command) : TryExecuteCurrent(context, command)))
            return false;

        e.Handled = true;
        return true;
    }

    public static bool TryExecute(ICommandContext? context, CommandId command)
    {
        for (var current = context; current != null; current = current.ParentCommandContext)
        {
            if (!current.CanExecuteCommand(command))
                continue;

            return current.ExecuteCommand(command);
        }

        return false;
    }

    static bool TryExecuteCurrent(ICommandContext? context, CommandId command)
    {
        if (context == null || !context.CanExecuteCommand(command))
            return false;

        return context.ExecuteCommand(command);
    }
}
