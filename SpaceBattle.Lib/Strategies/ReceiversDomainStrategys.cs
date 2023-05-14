namespace SpaceBattle.Lib;

public class ReceiversDomainStrategy : IStrategy
{
    public ConcurrentDictionary<String, ReceiverAdapter> ReceiversDomain;

    public ReceiversDomainStrategy() => ReceiversDomain = new ConcurrentDictionary<String, ReceiverAdapter>();

    public object Execute(object[] args)
    {
       return ReceiversDomain;
    }
}

public class ReceiversDomainGetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];

        var dict = IoC.Resolve<ConcurrentDictionary<string, ReceiverAdapter>>("Game.Receivers.Domain");
        return dict[id];
    }
}

public class ReceiversDomainSetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];
        ReceiverAdapter Receiver = (ReceiverAdapter) args[1];

        var SetCommand = new ReceiversDomainSetCommand(id, Receiver);
        return SetCommand;
    }
}
