namespace SpaceBattle.Lib;

public class FindHandlerWithTreeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var Command = (ICommand) args[0];
        var exception = (Exception) args[1];

        var ExceptionHandlerTree = IoC.Resolve<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>("GetExceptionTree");
        return ExceptionHandlerTree[Command][exception];
    }
}
