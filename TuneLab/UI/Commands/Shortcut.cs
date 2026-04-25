using Avalonia.Input;
using TuneLab.GUI.Input;

namespace TuneLab.UI.Commands;

internal readonly record struct Shortcut(Key Key, ModifierKeys Modifiers = ModifierKeys.None);
