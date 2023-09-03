using SpaceBattle.Lib;
using SpaceBattle.Lib.Test;
class Program
{
    static void Main(string[] args)
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        
        ServerThreadRegistryClass.ServerThreadRegistry();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new ActionCommand(() => {
            new StartServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new ActionCommand(() => {
            new StopServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new ActionCommand(() => {
            new HandlerCommand((string) args[0]).Execute();
        })).Execute();

        if (args[0] == "--thread")
        {
            Console.WriteLine("Starting . . .");
            int n_threads = int.Parse(args[1]);
            var server = new ConsoleServer(n_threads);
            server.Execute();
        }
        else
        {
            Console.WriteLine("No '--thread' argument specified.");
        }
    }
}
