namespace SpaceBattle.Lib;

public class ExceptionHandleCommand : ICommand
{
    private ICommand command;
    private Exception e;
    
    public ExceptionHandleCommand(ICommand command, Exception exception)
    {
        this.command = command;
        this.e = exception;
    }

    public void Execute()
    {   
        IStrategy FindExcHandlerForCommands = IoC.Resolve<IStrategy>("Game.Exceptions.FindExcHandlerForCommands", command, e);
        FindExcHandlerForCommands.Execute();
    }
}