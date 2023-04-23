namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    private int n_threads;

    public StopServerCommand(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {
    }
}