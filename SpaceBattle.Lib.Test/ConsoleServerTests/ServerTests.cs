namespace SpaceBattle.Lib.Test;

public class StopServerTests
{
    int n_threads = 5;
    string exception_message = "SOLID";
    public StopServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var MockThreadStrategy = new Mock<IStrategy>();
        MockThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(command.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new StartServerStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new StopServerStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new HandlerStrategy().Execute(args)).Execute();
    }

    [Fact]
    public void StartServerCommandTest()
    {
        ICommand cmd = new StartServerCommand(n_threads);
        cmd.Execute();
    }

    [Fact]
    public void StopServerCommandTest()
    {
        ICommand cmd = new StopServerCommand(n_threads);
        cmd.Execute();
    }

    [Fact]
    public void ConsoleServerTest()
    {
        var server = new ConsoleServer(n_threads);
        server.Execute();
    }

    [Fact]
    public void StartServerStrategyTest()
    {
        var str = new StartServerStrategy().Execute(n_threads);
    }

    [Fact]
    public void StopServerStrategyTest()
    {
        var str = new StopServerStrategy().Execute(n_threads);
    }

    [Fact]
    public void HandlerTest()
    {
        try
        {
            throw new Exception(exception_message);
        }
        catch (Exception e)
        {
            IoC.Resolve<ICommand>("CatchException", e.Message);
        }
    }
}