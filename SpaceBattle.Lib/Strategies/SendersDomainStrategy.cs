namespace SpaceBattle.Lib;

public class SendersDomainStrategy : IStrategy
{
    public ConcurrentDictionary<String, ISender> SendersDomain;

    public SendersDomainStrategy() => SendersDomain = new ConcurrentDictionary<String, ISender>();

    public object Execute(object[] args)
    {
       return SendersDomain;
    }
}
