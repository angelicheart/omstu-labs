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
        for (int id = 0; id < n_threads; id++)
        {
            IoC.Resolve<ICommand>("Soft Stop The Thread", id).Execute();
            Console.WriteLine("Thread №" + id + " stopped. . .");
        }
        Console.WriteLine("All threads are killed. Done!");
    }
}
