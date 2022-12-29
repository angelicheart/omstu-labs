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

        var SubTree = IoC.Resolve<IReadOnlyDictionary<Int32, IStrategy>>("Game.Exception.GetSubTree");

        return ExceptionHandlerTree[HashOfCommand].TryGetValue(HashOfException, out var value) ? value : SubTree.GetValueOrDefault(HashOfException);
    }
}
