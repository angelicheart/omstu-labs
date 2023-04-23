namespace SpaceBattle.Lib;

public class StartServerСommand : ICommand
{
    private int n_threads;

    public StartServerСommand(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {
        // создание потоков
    }
}