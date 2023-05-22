namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    object scope;
    Queue<ICommand> gameQueue;
    
    public GameCommand(object scope, Queue<ICommand> gameQueue)
    {
        this.scope = scope;
        this.gameQueue = gameQueue;
    }

    public void Execute()
    {
        
    }
}
