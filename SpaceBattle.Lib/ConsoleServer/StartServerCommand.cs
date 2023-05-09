namespace SpaceBattle.Lib;

public class StartServerCommand : ICommand
{
    private int n_threads;

    public StartServerCommand(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {
        for (int id = 0; id < n_threads; id++)
        {
            IoC.Resolve<ICommand>("Create And Start Thread", id).Execute();
            Console.WriteLine("Thread №" + id + " starting. . .");
        }
        Console.WriteLine("All threads are running. Done!");
    }
}
