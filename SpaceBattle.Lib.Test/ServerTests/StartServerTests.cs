namespace SpaceBattle.Lib.Test;

public class StartServerTests
{
    public StartServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new StartServerStrategy().Execute(args)).Execute();
        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) => Mock.Object.Execute(args)).Execute()
    }

    [Fact]
    public void Test1()
    {
        IoC.Resolve<ICommand>("StartServerCommand", 6).Execute();
    }

    [Fact]
    public void Test2()
    {
        var serv = new StartServer(6);
        serv.Execute();
    }
}
