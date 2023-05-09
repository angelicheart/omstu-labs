using SpaceBattle.Lib;
class Program
{
    static void Main(string[] args)
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        
        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var MockThreadStrategy = new Mock<IStrategy>();
        MockThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(command.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new StartServerStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new StopServerStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new HandlerStrategy().Execute(args)).Execute();

        if (args[0] == "--thread")
        {
            Console.WriteLine("Starting . . .");
            int n_threads = int.Parse(args[1]);
            var server = new ConsoleServer(n_threads);
            server.Execute();
        }
    }
}
