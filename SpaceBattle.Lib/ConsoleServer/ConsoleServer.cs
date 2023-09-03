namespace SpaceBattle.Lib;

public class ConsoleServer
{
    private int n_threads;

    public ConsoleServer(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {
        Console.WriteLine("Starting the server...");
        IoC.Resolve<ICommand>("StartServerCommand", n_threads).Execute();
        Console.WriteLine("Waiting for a press key . . .");
        Console.Read();
        Console.WriteLine("Stopping the server...");
        IoC.Resolve<ICommand>("StopServerCommand", n_threads).Execute();
    }
}
