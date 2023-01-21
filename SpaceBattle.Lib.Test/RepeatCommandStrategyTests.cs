namespace SpaceBattle.Lib.Test;

public class RepeatCommandStrategyTests
{
    public RepeatCommandStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var Command = new Mock<SpaceBattle.Lib.ICommand>();
        Command.Setup(c => c.Execute());

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(Command.Object);

        var InjectCommand = new InjectCommand(Command.Object);
        var InjectStrategy = new Mock<IStrategy>();
        InjectStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(InjectCommand);

        var RepeatCommand = new RepeatCommand(Command.Object);
        var RepeatStrategy = new Mock<IStrategy>();
        RepeatStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(RepeatCommand);

        var QueueCommand = new Mock<ICommand>();
        var QueueStrategy = new Mock<IStrategy>();
        QueueStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(QueueCommand.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.MacroCommand", (object[] args) => CommandStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.InjectCommand", (object[] args) => InjectStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.RepeatCommand", (object[] args) => RepeatStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => QueueStrategy.Object.Execute(args)).Execute();
    }

    [Fact]
    public void RepeatCommandStrategyPositive()
    {
        // Arrange
        var TestObj = new Mock<IUObject>();
        var Operation = "Operation.Name";

        // Act
        // Assert
        var Command = new RepeatCommandStrategy();
        Command.Execute(Operation, TestObj.Object);
    }

    [Fact]  
    public void RepeatCommandStrategyNullTest() 
    {
        // Arrange
        var TestObj = new Mock<IUObject>();
        var Command = new RepeatCommandStrategy();

        Command.Execute(null, TestObj.Object);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => Command.Execute());
    }


    [Fact]  
    public void InjectCommandPositiveTest() 
    {
        // Arrange
        var TestCommand = new Mock<ICommand>();
        var Command = new InjectCommand(TestCommand.Object);

        // Act
        // Assert
        Command.Execute();
    }

    [Fact]
    public void InjectCommandNullTest() 
    {
        // Arrange
        var TestObj = new Mock<IUObject>();
        var Command = new InjectCommand(null);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => Command.Execute());
    }

    [Fact]  
    public void RepeatCommandNullTest() 
    {
        // Arrange
        var TestObj = new Mock<IUObject>();
        var Command = new RepeatCommand(null);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => Command.Execute());
    }

    [Fact]  
    public void InjectMethodPositiveTest() 
    {
        // Arrange
        var Command = new Mock<ICommand>();
        Command.Setup(c => c.Execute());
        var InjectCmd = new InjectCommand(Command.Object);
        

        // Act
        // Assert
        InjectCmd.Inject(Command.Object);
        InjectCmd.Execute();
    }

    [Fact]  
    public void RepeatCommandPositiveTest() 
    {
        // Arrange
        var TestCommand = new Mock<ICommand>();
        var Command = new RepeatCommand(TestCommand.Object);

        // Act
        // Assert
        Command.Execute();
    }
}
