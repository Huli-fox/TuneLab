using System.Collections.Generic;
using System.Linq;

namespace TuneLab.UI.Commands;

internal static class CommandMetadataRegistry
{
    static readonly IReadOnlyDictionary<CommandId, CommandMetadata> sMetadata = new Dictionary<CommandId, CommandMetadata>
    {
        [CommandId.AppToggleFullScreen] = new(CommandId.AppToggleFullScreen, CommandCategory.App, "Full Screen"),

        [CommandId.TransportPlayPause] = new(CommandId.TransportPlayPause, CommandCategory.Transport, "Play/Pause"),
        [CommandId.TransportGoToStart] = new(CommandId.TransportGoToStart, CommandCategory.Transport, "Go to Start"),
        [CommandId.TransportGoToEnd] = new(CommandId.TransportGoToEnd, CommandCategory.Transport, "Go to End"),

        [CommandId.EditUndo] = new(CommandId.EditUndo, CommandCategory.Edit, "Undo"),
        [CommandId.EditRedo] = new(CommandId.EditRedo, CommandCategory.Edit, "Redo"),
        [CommandId.EditSwitchLastPart] = new(CommandId.EditSwitchLastPart, CommandCategory.Edit, "Switch Last Part"),
        [CommandId.EditOpenSettings] = new(CommandId.EditOpenSettings, CommandCategory.Edit, "Open Settings"),

        [CommandId.FileNewProject] = new(CommandId.FileNewProject, CommandCategory.File, "New Project"),
        [CommandId.FileOpenProject] = new(CommandId.FileOpenProject, CommandCategory.File, "Open Project"),
        [CommandId.FileSaveProject] = new(CommandId.FileSaveProject, CommandCategory.File, "Save Project"),
        [CommandId.FileSaveProjectAs] = new(CommandId.FileSaveProjectAs, CommandCategory.File, "Save Project As"),
        [CommandId.FileExportMix] = new(CommandId.FileExportMix, CommandCategory.File, "Export Mix"),

        [CommandId.SelectionDelete] = new(CommandId.SelectionDelete, CommandCategory.Selection, "Delete Selection"),
        [CommandId.SelectionCopy] = new(CommandId.SelectionCopy, CommandCategory.Selection, "Copy Selection"),
        [CommandId.SelectionCut] = new(CommandId.SelectionCut, CommandCategory.Selection, "Cut Selection"),
        [CommandId.SelectionPaste] = new(CommandId.SelectionPaste, CommandCategory.Selection, "Paste Selection"),
        [CommandId.SelectionSelectAll] = new(CommandId.SelectionSelectAll, CommandCategory.Selection, "Select All"),

        [CommandId.PianoTransposeUp] = new(CommandId.PianoTransposeUp, CommandCategory.Piano, "Transpose Up"),
        [CommandId.PianoTransposeDown] = new(CommandId.PianoTransposeDown, CommandCategory.Piano, "Transpose Down"),
        [CommandId.PianoOctaveUp] = new(CommandId.PianoOctaveUp, CommandCategory.Piano, "Octave Up"),
        [CommandId.PianoOctaveDown] = new(CommandId.PianoOctaveDown, CommandCategory.Piano, "Octave Down"),
        [CommandId.PianoLyricNextNote] = new(CommandId.PianoLyricNextNote, CommandCategory.Piano, "Next Lyric Note"),
        [CommandId.PianoLyricPreviousNote] = new(CommandId.PianoLyricPreviousNote, CommandCategory.Piano, "Previous Lyric Note"),
        [CommandId.PianoToolNote] = new(CommandId.PianoToolNote, CommandCategory.Piano, "Note Tool"),
        [CommandId.PianoToolPitch] = new(CommandId.PianoToolPitch, CommandCategory.Piano, "Pitch Tool"),
        [CommandId.PianoToolAnchor] = new(CommandId.PianoToolAnchor, CommandCategory.Piano, "Anchor Tool"),
        [CommandId.PianoToolLock] = new(CommandId.PianoToolLock, CommandCategory.Piano, "Pitch Lock Tool"),
        [CommandId.PianoToolVibrato] = new(CommandId.PianoToolVibrato, CommandCategory.Piano, "Vibrato Tool"),
        [CommandId.PianoToolSelect] = new(CommandId.PianoToolSelect, CommandCategory.Piano, "Selection Tool"),
    };

    public static bool TryGet(CommandId command, out CommandMetadata metadata)
    {
        return sMetadata.TryGetValue(command, out metadata);
    }

    public static IReadOnlyCollection<CommandMetadata> GetAll()
    {
        return sMetadata.Values.ToArray();
    }
}
