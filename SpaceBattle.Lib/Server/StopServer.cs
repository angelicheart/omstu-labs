namespace SpaceBattle.Lib;

public class StopServer : ICommand
{
    private int n_threads;

    public StopServer(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {

    }
}