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
        // realization
        return 0;
    }
}