namespace SpaceBattle.Lib.Test;

public class StopServerTests
{
    int n_threads = 5;

    public StopServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var MockThreadStrategy = new Mock<IStrategy>();
        MockThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(command.Object);

        var StopServer = new StopServerStrategy(); 

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => StopServer.Execute(args)).Execute();
    }

    [Fact]
    public void StopConsoleServerCommandTest()
    {
        var cmd = new StopServerCommand(n_threads);
        cmd.Execute();
    }

    [Fact]
    public void StopConsoleServerStrategyTest()
    {
        var str = new StopServerStrategy();
        str.Execute(n_threads);
    }

    [Fact]
    public void StopServerCommandTest()
    {
        IoC.Resolve<ICommand>("StopServerCommand", n_threads).Execute();
    }
}
