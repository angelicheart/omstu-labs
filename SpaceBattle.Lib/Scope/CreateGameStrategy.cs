namespace SpaceBattle.Lib;

public class CreateGameStrategy : IStrategy
{
    readonly int quant;
    readonly string id;

    public CreateGameStrategy(string id, int quant)
    {
        this.quant = quant;
        this.id = id;
    }
    
    public object Execute(params object[] args)
    {
        Queue<ICommand> queue = new Queue<ICommand>();
        object newscope = new InitializeScopeStrategy().Execute(id, quant);

        return IoC.Resolve<ICommand>("Commands.GameCommand", newscope, queue);
    }
}
