namespace SpaceBattle.Lib;

public class StartServerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new StartServerСommand((int)args[0]);
    }
}