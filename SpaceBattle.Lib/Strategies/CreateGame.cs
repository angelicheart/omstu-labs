namespace SpaceBattle.Lib;

public class CreateGame : IStrategy 
{
    int id, quant;

    public CreateGame(int id, int quant) {
        this.id = id;
        this.quant = quant;
    }

    public object Execute(params object[] args) 
    {
        Queue<ICommand> queue = new Queue<ICommand>();
        var scope = new InitializeScope().Execute(id, quant);

        return IoC.Resolve<ICommand>("GameCommand", scope, queue);
    }
}