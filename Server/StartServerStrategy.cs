namespace SpaceBattle.Lib;

public class StartServerStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new StartServerCommand((int)args[0]);
    }
}
