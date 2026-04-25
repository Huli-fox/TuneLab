using System;
using System.Collections.Generic;

namespace TuneLab.UI.Commands;

internal sealed class CommandMap
{
    readonly IReadOnlyDictionary<CommandId, Action> mHandlers;

    public CommandMap(IReadOnlyDictionary<CommandId, Action> handlers)
    {
        mHandlers = handlers;
    }

    public bool Contains(CommandId command)
    {
        return mHandlers.ContainsKey(command);
    }

    public bool TryExecute(CommandId command)
    {
        if (!mHandlers.TryGetValue(command, out var handler))
            return false;

        handler();
        return true;
    }
}
