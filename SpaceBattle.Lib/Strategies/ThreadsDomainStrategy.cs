namespace SpaceBattle.Lib;

public class ThreadsDomainStrategy : IStrategy
{
    public Dictionary<String, ServerThread> ThreadsDomain;

    public ThreadsDomainStrategy() => ThreadsDomain = new Dictionary<String, ServerThread>();

    public object Execute(object[] args)
    {
       return ThreadsDomain;
    }
}
