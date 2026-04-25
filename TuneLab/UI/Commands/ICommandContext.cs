namespace TuneLab.UI.Commands;

internal interface ICommandContext
{
    ICommandContext? ParentCommandContext { get; }
    bool CanExecuteCommand(CommandId command);
    bool ExecuteCommand(CommandId command);
}
