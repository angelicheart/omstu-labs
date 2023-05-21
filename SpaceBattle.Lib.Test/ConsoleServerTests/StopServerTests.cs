namespace SpaceBattle.Lib.Test;

public class StopServerTests
{
    int n_threads = 5;

    public StopServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        ServerThreadRegistryClass.ServerThreadRegistry();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new ActionCommand(() => {
            new StopServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new ActionCommand(() => {
            new HandlerCommand((string) args[0]).Execute();
        })).Execute();
    }

    [Fact]
    public void StopConsoleServerCommandTest()
    {
        var cmd = new StopServerCommand(n_threads);
        cmd.Execute();
    }

    [Fact]
    public void StopServerCommandTest()
    {
        IoC.Resolve<ICommand>("StopServerCommand", n_threads).Execute();
    }
}
