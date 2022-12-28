namespace SpaceBattle.Lib.Test;

public class FindExceptionHandlerCommandTests
{
    public FindExceptionHandlerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
                        
        var CatchesException = new Mock<IStrategy>();
        CatchesException.Setup(ce => ce.Execute(It.IsAny<object[]>())).Returns((object) true);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CatchesExceptions", (object[] args) => CatchesException.Object.Execute()).Execute();
    }

    [Fact]
    public void CommandNoThrowsExceptionTest()
    {
        // Arrange
        var CommandsQueue = new Mock<Queue<ICommand>>();

        var Command1 = new Mock<ICommand>();
        var Command2 = Command1;

        CommandsQueue.Object.Enqueue(Command1.Object);
        CommandsQueue.Object.Enqueue(Command2.Object);

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s1 => s1.Execute(It.IsAny<object[]>())).Returns(Command1.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindHandlerWithTree", (object[] args) => CommandStrategy.Object.Execute(args)).Execute();

        var FindExceptionCommand = new FindExceptionCommand(CommandsQueue.Object);
        
        // Act
        // Assert
        FindExceptionCommand.Execute();
    }

    [Fact]
    public void CommandThrowsExceptionTest()
    {
        var CommandsQueue = new Mock<Queue<ICommand>>();

        var Command1 = new Mock<ICommand>();
        Command1.Setup(c1 => c1.Execute()).Throws(new Exception());
        var Command2 = Command1;

        CommandsQueue.Object.Enqueue(Command1.Object);
        CommandsQueue.Object.Enqueue(Command2.Object);

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s1 => s1.Execute(It.IsAny<object[]>())).Returns(Command1.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindHandlerWithTree", (object[] args) => CommandStrategy.Object).Execute();

        var FindExceptionCommand = new FindExceptionCommand(CommandsQueue.Object);
        
        // Act
        // Assert
        FindExceptionCommand.Execute();
    }
}
