namespace SpaceBattle.Lib;

public class DefaultExceptionHandlerCommand : ICommand
{
    private ICommand command;
    private Exception exception;

    public DefaultExceptionHandlerCommand(ICommand command, Exception exception)
    {
        this.command = command;
        this.exception = exception;
    }
    
    public void Execute()
    {   
        IDictionary<ICommand, Exception> excDict = IoC.Resolve<IDictionary<ICommand, Exception>>("Game.ICommand_Exception.Dict.Get");
        excDict.Add(command, exception);
    }
}
