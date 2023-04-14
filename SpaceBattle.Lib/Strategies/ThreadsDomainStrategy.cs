namespace SpaceBattle.Lib;

public class ThreadsDomainStrategy : IStrategy
{
    public ConcurrentDictionary<String, ServerThread> ThreadsDomain;

    public ThreadsDomainStrategy() => ThreadsDomain = new ConcurrentDictionary<String, ServerThread>();

    public object Execute(object[] args)
    {
       return ThreadsDomain;
    }
}
