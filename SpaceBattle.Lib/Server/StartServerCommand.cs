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
        //for (int id = 0; id < n_threads; id++)
        //{
            // IoC.Resolve<ICommand>("Create And Start Thread", id).Execute();
        //}
    }
}