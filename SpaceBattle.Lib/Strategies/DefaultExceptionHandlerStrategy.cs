namespace SpaceBattle.Lib;

public class DefaultExceptionHandlerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        ICommand Command = (ICommand) args[0];
        Exception exception = (Exception) args[1];

        return new DefaultExceptionHandlerCommand(Command, exception);
    }
}
