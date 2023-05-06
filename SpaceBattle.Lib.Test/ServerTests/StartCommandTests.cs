namespace SpaceBattle.Lib.Test;

public class StartServerTests
{
    public StartServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        //var command = new Mock<SpaceBattle.Lib.ICommand>();
        //command.Setup(c => c.Execute());

        //var MockThreadStrategy = new Mock<IStrategy>();
        //MockThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(command.Object);

        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new StartServerStrategy().Execute(args)).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new StopServerStrategy().Execute(args)).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) => MockThreadStrategy.Object.Execute(args)).Execute();
    }

    [Fact]
    public void Test1()
    {
        //ICommand cmd = new StartServerCommand(6);
        //cmd.Execute();
    }

    [Fact]
    public void Test2()
    {
        //ICommand cmd = new StopServerCommand(6);
        //cmd.Execute();
    }
}