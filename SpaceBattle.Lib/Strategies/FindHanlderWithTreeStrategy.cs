namespace SpaceBattle.Lib;

public class FindHandlerWithTreeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        ICommand Command = (ICommand) args[0];
        Exception exception = (Exception) args[1];

        var ExceptionHandlerTree = IoC.Resolve<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>("GetExceptionTree");
        return ExceptionHandlerTree[Command][exception];
    }
}
