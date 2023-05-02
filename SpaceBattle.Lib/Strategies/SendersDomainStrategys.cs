namespace SpaceBattle.Lib;

public class SendersDomainStrategy : IStrategy
{
    public ConcurrentDictionary<String, SenderAdapter> SendersDomain;

    public SendersDomainStrategy() => SendersDomain = new ConcurrentDictionary<String, SenderAdapter>();

    public object Execute(object[] args)
    {
       return SendersDomain;
    }
}

public class SendersDomainGetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];

        var dict = IoC.Resolve<ConcurrentDictionary<string, SenderAdapter>>("Game.Senders.Domain");
        return dict[id];
    }
}

public class SendersDomainSetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];
        SenderAdapter sender = (SenderAdapter) args[1];

        var SetCommand = new SendersDomainSetCommand(id, sender);
        return SetCommand;
    }
}
