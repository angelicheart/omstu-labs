namespace SpaceBattle.Lib.Test;

public class ConsoleServerTests
{
    const int n_threads = 3;

    int n_starts = 0, n_stops = 0;

    public ConsoleServerTests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var start = new Mock<SpaceBattle.Lib.ICommand>();
        start.Setup(c => c.Execute()).Callback(() => { n_starts++; });

        var StartThreadStrategy = new Mock<IStrategy>();
        StartThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(start.Object).Verifiable();

        var send = new Mock<SpaceBattle.Lib.ICommand>();
        send.Setup(c => c.Execute()).Callback(() => { n_stops++; });

        var SendStrategy = new Mock<IStrategy>();
        SendStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(send.Object).Verifiable();

        var stop = new Mock<SpaceBattle.Lib.ICommand>();
        stop.Setup(c => c.Execute());

        var StopThreadStrategy = new Mock<IStrategy>();
        StopThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(stop.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.CreateAndStart", (object[] args) => StartThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.SoftStop", (object[] args) => StopThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Send", (object[] args) => SendStrategy.Object.Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new ActionCommand(() => {
            new StartServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new ActionCommand(() => {
            new StopServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new ActionCommand(() => {
            new HandlerCommand((string) args[0]).Execute();
        })).Execute();
    }
    
    [Fact]
    public void ConsoleServerTest()
    {
        var server = new ConsoleServer(3);
        server.Execute();

        Assert.Equal(n_threads, n_stops);
        Assert.Equal(n_threads, n_starts);
    }

    [Fact]
    public void StartServerCommandTest()
    {
        n_starts = 0;

        var cmd = new StartServerCommand(n_threads);
        cmd.Execute();

        Assert.Equal(n_threads, n_starts);
    }

    [Fact]
    public void StopServerCommandTest()
    {
        n_stops = 0;

        var cmd = new StopServerCommand(n_threads);
        cmd.Execute();

        Assert.Equal(n_threads, n_stops);
    }
}
