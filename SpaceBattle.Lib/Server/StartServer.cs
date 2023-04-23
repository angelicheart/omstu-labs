namespace SpaceBattle.Lib;

public class StartServer
{
    private int n_threads;

    public StartServer(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("StartServerCommand", n_threads).Execute();
        Console.WriteLine("Ожидание нажатия клавиши");
        Console.ReadKey();
        // IoC.Resolve<ICommand>("StopServerCommand", n_threads).Execute();
    }
}
