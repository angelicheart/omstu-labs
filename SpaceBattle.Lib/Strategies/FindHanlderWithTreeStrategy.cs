namespace SpaceBattle.Lib;

public class FindHandlerWithTreeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        Int32 Command = (Int32) args[0];
        Int32 exception = (Int32) args[1];

        var ExceptionHandlerTree = IoC.Resolve<IDictionary<Int32, IDictionary<Int32, IStrategy>>>("GetExceptionTree");
        return ExceptionHandlerTree[Command][exception];
    }
}
