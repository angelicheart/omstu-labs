namespace SpaceBattle.Lib;

public class RecieversDomainStrategy : IStrategy
{
    public ConcurrentDictionary<String, IReciever> RecieversDomain;

    public RecieversDomainStrategy() => RecieversDomain = new ConcurrentDictionary<String, IReciever>();

    public object Execute(object[] args)
    {
       return RecieversDomain;
    }
}
