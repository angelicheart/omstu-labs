namespace SpaceBattle.Lib;

public class HandlerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new HandlerCommand((string)args[0]);
    }
}
