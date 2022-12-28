namespace SpaceBattle.Lib.Test;

public class FindExceptionHandlerCommandTests
{
    public FindExceptionHandlerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CommandNoThrowsExceptionTest()
    {
        var GameIsRunning = new Mock<IStrategy>();
        GameIsRunning.Setup(crt => crt.Execute(It.IsAny<object[]>())).Returns((object) true);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CatchesExceptions", (object[] args) => GameIsRunning.Object.Execute(args)).Execute();

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Throws(new Exception());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindHandlerWithTree", (object[] args) => CommandStrategy.Object.Execute()).Execute();
        
        var Command = new Mock<SpaceBattle.Lib.ICommand>();
        Command.Setup(c => c.Execute());

        var FindExceptionCommand = new FindExceptionCommand(Command.Object);
        
        FindExceptionCommand.Execute();
    }

    [Fact]
    public void CommandThrowsExceptionTest()
    {
        var GameIsRunning = new Mock<IStrategy>();
        GameIsRunning.Setup(crt => crt.Execute(It.IsAny<object[]>())).Returns((object) true);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CatchesExceptions", (object[] args) => GameIsRunning.Object.Execute(args)).Execute();

        var ExceptionHandlerCommand = new Mock<IStrategy>();

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(ExceptionHandlerCommand.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindHandlerWithTree", (object[] args) => CommandStrategy.Object.Execute()).Execute();
        
        var Command = new Mock<SpaceBattle.Lib.ICommand>();
        Command.Setup(c => c.Execute()).Throws(new Exception());

        var FindExceptionCommand = new FindExceptionCommand(Command.Object);
        
        FindExceptionCommand.Execute();
    }

    [Fact]
    public void GameIsNotRunningTest()
    {
        var GameIsRunning = new Mock<IStrategy>();
        GameIsRunning.Setup(crt => crt.Execute(It.IsAny<object[]>())).Returns((object) false);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CatchesExceptions", (object[] args) => GameIsRunning.Object.Execute(args)).Execute();

        var Command = new Mock<SpaceBattle.Lib.ICommand>();
        Command.Setup(c => c.Execute());

        var FindExceptionCommand = new FindExceptionCommand(Command.Object);
        
        Assert.ThrowsAny<Exception>(() => FindExceptionCommand.Execute());
    }
}
