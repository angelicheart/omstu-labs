namespace SpaceBattle.Lib;
using Hwdtech;
public class DeleteGameStrategy : IStrategy
{
    string scopeid;
    public DeleteGameStrategy(string scopeid)
    {
        this.scopeid = scopeid;
    }
    public object Execute(params object[] args)
    {
        IoC.Resolve<ICommand>("Game.DeleteScope", scopeid).Execute();
        return new object();
    }
}
