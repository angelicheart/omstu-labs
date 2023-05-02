namespace SpaceBattle.Lib;

public class StopServerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new StopServerCommand((int)args[0]);
    }
}
