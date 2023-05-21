namespace SpaceBattle.Lib.Test;

public class StartServerTests
{
    int n_threads = 5;

    public StartServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        ServerThreadRegistryClass.ServerThreadRegistry();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new ActionCommand(() => {
            new StartServerCommand((int) args[0]).Execute();
        })).Execute();
    }


    [Fact]
    public void StartConsoleServerCommandTest()
    {
        var cmd = new StartServerCommand(n_threads);
        cmd.Execute();
    }

    [Fact]
    public void StartServerCommandTest()
    {
        IoC.Resolve<ICommand>("StartServerCommand", n_threads).Execute();
    }
}
