namespace SpaceBattle.Lib;

public class StopServerTests
{
    [Fact]
    public void StopThreads()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Stop Server Command", (object[] args) => StopServerStrategy().Execute(args)).Execute();
        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Hard Stop The Thread", (object[] args) => MockStategy.Object.Execute(args)).Execute()'

    }
}