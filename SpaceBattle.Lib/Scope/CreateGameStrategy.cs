namespace SpaceBattle.Lib;
using Hwdtech;

public class CreateGameStrategy : IStrategy
{
    int quantum;
    string scopeid;
    public CreateGameStrategy(string scopeid, int quantum = 500)
    {
        this.quantum = quantum;
        this.scopeid = scopeid;
    }
    public object Execute(params object[] args)
    {
        Queue<ICommand> queue = new Queue<ICommand>();
        object newscope = new InitializeScopeStrategy().Execute(scopeid, quantum);

        return IoC.Resolve<ICommand>("Commands.GameCommand", newscope, queue);
    }
}
