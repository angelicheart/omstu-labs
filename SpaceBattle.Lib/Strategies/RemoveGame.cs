namespace SpaceBattle.Lib;

public class RemoveGame : ICommand
{
    int id;
    public RemoveGame(int id) 
    {
        this.id = id;
    }

    public void Execute() {
        // realization
    }
}