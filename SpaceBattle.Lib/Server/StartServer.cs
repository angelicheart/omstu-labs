namespace SpaceBattle.Lib;

public class StartServer : ICommand
{
    private int n_threads;

    public StartServer(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {

    }
}