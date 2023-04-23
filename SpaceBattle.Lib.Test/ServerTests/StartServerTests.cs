namespace SpaceBattle.Lib;

public class StartServerTests
{
    [Fact]
    public void StartThreads()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new StartServerStrategy().Execute(args)).Execute();
        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateThreads", (object[] args) => Mock.Object.Execute(args)).Execute()'

    }
}