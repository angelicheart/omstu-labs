namespace SpaceBattle.Lib.Test;

public class StartServerTests
{
    int n_threads = 5;

    public StartServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var MockThreadStrategy = new Mock<IStrategy>();
        MockThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(command.Object);

        var StartServer = new StartServerStrategy(); 

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => StartServer.Execute(args)).Execute();
    }

    [Fact]
    public void StartServerCommandTest()
    {
        IoC.Resolve<ICommand>("StartServerCommand", n_threads).Execute();
    }
}
