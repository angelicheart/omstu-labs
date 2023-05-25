namespace SpaceBattle.Lib;

public class CommandExceptionStorageStrategy : IStrategy
{
    public Dictionary<ICommand, Exception> CommandExceptionStorage;

    public CommandExceptionStorageStrategy() => CommandExceptionStorage = new Dictionary<ICommand, Exception>();

    public object Execute(object[] args)
    {
       return CommandExceptionStorage;
    }
}
