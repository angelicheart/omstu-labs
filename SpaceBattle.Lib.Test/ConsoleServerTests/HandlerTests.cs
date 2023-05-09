namespace SpaceBattle.Lib.Test;

public class HandlerTests
{
    string exception_message = "SOLID.Exception";

    public HandlerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new HandlerStrategy().Execute(args)).Execute();
    }

    [Fact]
    public void CatchExceptionTest()
    {
        IoC.Resolve<ICommand>("CatchException", exception_message).Execute();
    }
}
