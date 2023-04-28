namespace SpaceBattle.Lib;

class Program
{
    static void Main(string[] args)
    {   
        int n_threads = int.Parse(args[0]);
        var server = new StartServer(n_threads);
        server.Execute();
    }
}
