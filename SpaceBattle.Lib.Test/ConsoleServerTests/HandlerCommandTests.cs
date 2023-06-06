namespace SpaceBattle.Lib.Test;

public class HandlerTests
{
    string exception_message = "SOLID.Exception";

    public HandlerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.CreateAndStart", (object[] args) =>
        { 
            Mock<Exception> ex = new(); 
            return ex.Object; 
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.SoftStop", (object[] args) =>
        { 
            Mock<Exception> ex = new(); 
            return ex.Object; 
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new ActionCommand(() => {
            new HandlerCommand((string) args[0]).Execute();
        })).Execute();
    }

    [Fact]
    public void HandlerCommandTest()
    {
        var handler = new HandlerCommand(exception_message);
        handler.Execute();
    }

    [Fact]
    public void CatchExceptionTest()
    {
        IoC.Resolve<ICommand>("CatchException", exception_message).Execute();
    }

    [Fact]
    public void StartServerCommandException()
    {
        var cmd = new StopServerCommand(5);
        cmd.Execute();
    }

    [Fact]
    public void StopServerCommandException()
    {
        var cmd = new StopServerCommand(5);
        cmd.Execute();
    }
}
