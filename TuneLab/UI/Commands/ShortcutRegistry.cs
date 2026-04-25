using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using TuneLab.GUI.Input;
using TuneLab.Utils;

namespace TuneLab.UI.Commands;

internal static class ShortcutRegistry
{
    static readonly IReadOnlyDictionary<CommandId, Shortcut> sDefaultShortcuts = new Dictionary<CommandId, Shortcut>
    {
        [CommandId.AppToggleFullScreen] = new(Key.F11),
        [CommandId.TransportPlayPause] = new(Key.Space),
        [CommandId.TransportGoToStart] = new(Key.Decimal),
        [CommandId.TransportGoToEnd] = new(Key.End),
        [CommandId.EditUndo] = new(Key.Z, ModifierKeys.Ctrl),
        [CommandId.EditRedo] = new(Key.Y, ModifierKeys.Ctrl),
        [CommandId.EditSwitchLastPart] = new(Key.Tab, ModifierKeys.Ctrl),
        [CommandId.EditOpenSettings] = new(Key.OemComma, ModifierKeys.Ctrl),
        [CommandId.FileNewProject] = new(Key.N, ModifierKeys.Ctrl),
        [CommandId.FileOpenProject] = new(Key.O, ModifierKeys.Ctrl),
        [CommandId.FileSaveProject] = new(Key.S, ModifierKeys.Ctrl),
        [CommandId.FileSaveProjectAs] = new(Key.S, ModifierKeys.Ctrl | ModifierKeys.Shift),
        [CommandId.FileExportMix] = new(Key.Enter, ModifierKeys.Ctrl),
        [CommandId.SelectionDelete] = new(Key.Delete),
        [CommandId.SelectionCopy] = new(Key.C, ModifierKeys.Ctrl),
        [CommandId.SelectionCut] = new(Key.X, ModifierKeys.Ctrl),
        [CommandId.SelectionPaste] = new(Key.V, ModifierKeys.Ctrl),
        [CommandId.SelectionSelectAll] = new(Key.A, ModifierKeys.Ctrl),
        [CommandId.PianoTransposeUp] = new(Key.Up),
        [CommandId.PianoTransposeDown] = new(Key.Down),
        [CommandId.PianoOctaveUp] = new(Key.Up, ModifierKeys.Shift),
        [CommandId.PianoOctaveDown] = new(Key.Down, ModifierKeys.Shift),
        [CommandId.PianoLyricNextNote] = new(Key.Tab),
        [CommandId.PianoLyricPreviousNote] = new(Key.Tab, ModifierKeys.Shift),
        [CommandId.PianoToolNote] = new(Key.D1),
        [CommandId.PianoToolPitch] = new(Key.D2),
        [CommandId.PianoToolAnchor] = new(Key.D3),
        [CommandId.PianoToolLock] = new(Key.D4),
        [CommandId.PianoToolVibrato] = new(Key.D5),
        [CommandId.PianoToolSelect] = new(Key.D6),
    };
    static readonly Dictionary<CommandId, Shortcut> sOverrides = [];

    public static event Action? ShortcutsChanged;

    public static bool TryGetShortcut(CommandId command, out Shortcut shortcut)
    {
        if (sOverrides.TryGetValue(command, out shortcut))
            return true;

        return sDefaultShortcuts.TryGetValue(command, out shortcut);
    }

    public static bool TryGetDefaultShortcut(CommandId command, out Shortcut shortcut)
    {
        return sDefaultShortcuts.TryGetValue(command, out shortcut);
    }

    public static bool HasOverride(CommandId command)
    {
        return sOverrides.ContainsKey(command);
    }

    public static IReadOnlyDictionary<CommandId, Shortcut> GetAll()
    {
        return sDefaultShortcuts.Keys.ToDictionary(command => command, GetShortcut);
    }

    public static IReadOnlyCollection<ShortcutOverrideFile> GetOverrides()
    {
        return sOverrides
            .OrderBy(pair => pair.Key)
            .Select(pair => new ShortcutOverrideFile()
            {
                Command = pair.Key.ToString(),
                Key = pair.Value.Key.ToString(),
                Modifiers = (int)pair.Value.Modifiers,
            })
            .ToArray();
    }

    public static void LoadOverrides(IEnumerable<ShortcutOverrideFile>? overrides)
    {
        sOverrides.Clear();
        if (overrides == null)
            return;

        foreach (var file in overrides)
        {
            if (!Enum.TryParse<CommandId>(file.Command, out var command))
                continue;

            if (!sDefaultShortcuts.ContainsKey(command))
                continue;

            if (!Enum.TryParse<Key>(file.Key, out var key))
                continue;

            var shortcut = new Shortcut(key, (ModifierKeys)file.Modifiers);
            if (shortcut != sDefaultShortcuts[command])
            {
                sOverrides[command] = shortcut;
            }
        }
    }

    public static bool SetShortcut(CommandId command, Shortcut shortcut)
    {
        if (!sDefaultShortcuts.TryGetValue(command, out var defaultShortcut))
            return false;

        bool changed;
        if (shortcut == defaultShortcut)
        {
            changed = sOverrides.Remove(command);
        }
        else if (sOverrides.TryGetValue(command, out var currentShortcut) && currentShortcut == shortcut)
        {
            return false;
        }
        else
        {
            sOverrides[command] = shortcut;
            changed = true;
        }

        if (changed)
        {
            ShortcutsChanged?.Invoke();
        }
        return changed;
    }

    public static bool ResetShortcut(CommandId command)
    {
        if (!sOverrides.Remove(command))
            return false;

        ShortcutsChanged?.Invoke();
        return true;
    }

    public static void ResetAll()
    {
        if (sOverrides.Count == 0)
            return;

        sOverrides.Clear();
        ShortcutsChanged?.Invoke();
    }

    public static bool TryFindCommand(Shortcut shortcut, out CommandId command, CommandId? excludedCommand = null)
    {
        foreach (var commandId in sDefaultShortcuts.Keys)
        {
            if (excludedCommand.HasValue && commandId == excludedCommand.Value)
                continue;

            if (GetShortcut(commandId) == shortcut)
            {
                command = commandId;
                return true;
            }
        }

        command = default;
        return false;
    }

    public static bool TryGetCommand(KeyEventArgs e, out CommandId command)
    {
        foreach (var commandId in sDefaultShortcuts.Keys)
        {
            var shortcut = GetShortcut(commandId);
            if (e.Match(shortcut.Key, shortcut.Modifiers))
            {
                command = commandId;
                return true;
            }
        }

        command = default;
        return false;
    }

    static Shortcut GetShortcut(CommandId command)
    {
        return sOverrides.TryGetValue(command, out var shortcut) ? shortcut : sDefaultShortcuts[command];
    }
}
