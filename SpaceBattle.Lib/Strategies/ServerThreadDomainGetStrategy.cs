namespace SpaceBattle.Lib;

public class ServerThreadDomainGetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];

        var dict = IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Game.Threads.Domain");
        return dict[id];
    }
}