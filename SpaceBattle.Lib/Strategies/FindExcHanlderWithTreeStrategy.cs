namespace SpaceBattle.Lib;

public class FindHandlerWithTreeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        ICommand Command = (ICommand) args[0];
        Exception exception = (Exception) args[1];

        Int32 HashOfCommand = Command.GetType().GetHashCode();
        Int32 HashOfException = exception.GetType().GetHashCode();

        var ExceptionHandlerTree = IoC.Resolve<IReadOnlyDictionary<Int32, IReadOnlyDictionary<Int32, IStrategy>>>("Game.Exception.GetTree");

        var ExceptionDict = IoC.Resolve<IReadOnlyDictionary<Int32, IStrategy>>("Game.Exception.GetSubTree");

        ExceptionHandlerTree.GetValueOrDefault(HashOfCommand, ExceptionDict);

        IStrategy Handler = IoC.Resolve<IStrategy>("Game.Exception.GetEmptyStrategy"); 

        return ExceptionDict.GetValueOrDefault(HashOfException, Handler);
    }
}
