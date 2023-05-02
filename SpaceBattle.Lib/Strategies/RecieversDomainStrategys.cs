namespace SpaceBattle.Lib;

public class RecieversDomainStrategy : IStrategy
{
    public ConcurrentDictionary<String, RecieverAdapter> RecieversDomain;

    public RecieversDomainStrategy() => RecieversDomain = new ConcurrentDictionary<String, RecieverAdapter>();

    public object Execute(object[] args)
    {
       return RecieversDomain;
    }
}

public class RecieversDomainGetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];

        var dict = IoC.Resolve<ConcurrentDictionary<string, RecieverAdapter>>("Game.Recievers.Domain");
        return dict[id];
    }
}

public class RecieversDomainSetStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string) args[0];
        RecieverAdapter reciever = (RecieverAdapter) args[1];

        var SetCommand = new RecieversDomainSetCommand(id, reciever);
        return SetCommand;
    }
}
