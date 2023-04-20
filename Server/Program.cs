namespace SpaceBattle.Lib;

class Program
{
    static void Main(string[] args)
    {   
        int n_threads = int.Parse(args[0]);
        var bla = new StartServer(n_threads);
    }
}