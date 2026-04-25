namespace TuneLab.UI.Commands;

// CommandId is reserved for user-invokable shortcut commands.
// Drag modifiers and one-off dialog keys should stay in their local interaction code.
internal enum CommandId
{
    AppToggleFullScreen,

    TransportPlayPause,
    TransportGoToStart,
    TransportGoToEnd,

    EditUndo,
    EditRedo,
    EditSwitchLastPart,
    EditOpenSettings,

    FileNewProject,
    FileOpenProject,
    FileSaveProject,
    FileSaveProjectAs,
    FileExportMix,

    SelectionDelete,
    SelectionCopy,
    SelectionCut,
    SelectionPaste,
    SelectionSelectAll,

    PianoTransposeUp,
    PianoTransposeDown,
    PianoOctaveUp,
    PianoOctaveDown,
    PianoLyricNextNote,
    PianoLyricPreviousNote,
    PianoToolNote,
    PianoToolPitch,
    PianoToolAnchor,
    PianoToolLock,
    PianoToolVibrato,
    PianoToolSelect,
}
