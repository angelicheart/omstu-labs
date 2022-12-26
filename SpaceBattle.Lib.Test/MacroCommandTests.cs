namespace SpaceBattle.Lib.Test;

public class MacroCommandTests
{ 
    string TestStrategyReturnsListOfOperations = "TestStrategy";
    public MacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var Command = new Mock<SpaceBattle.Lib.ICommand>();
        Command.Setup(c => c.Execute());

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(Command.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", TestStrategyReturnsListOfOperations, (object[] args) => new List<string> {"TestDep"}).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "TestDep", (object[] args) => CommandStrategy.Object.Execute(args)).Execute();
    }

    [Fact]
    public void PositiveMacroCommandTest()
    {   
        // Arrange
        var TestObj = new Mock<IUObject>();
        var Commands = new List<ICommand>();

        var StratList = IoC.Resolve<List<string>>(TestStrategyReturnsListOfOperations);

        StratList.ForEach(strat => Commands.Add(IoC.Resolve<ICommand>(strat, TestObj.Object)));

        // Act
        // Assert
        var MacroCommand = new MacroCommand(Commands);

        MacroCommand.Execute();
    }

    [Fact]  
    public void MacroCommandNullTest() 
    {
        // Arrange
        var MacroCommand = new MacroCommand(null);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => MacroCommand.Execute());
    }

    [Fact]
    public void MacroCommandListHasNullTest() 
    {
        // Arrange
        var Command = new Mock<SpaceBattle.Lib.ICommand>();

        var MacroCommand = new MacroCommand(new List<ICommand>{null!, Command.Object});

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => MacroCommand.Execute());
    }

    [Fact]
    public void PositiveMacroCommandStrategyTest()
    {   
        // Arrange
        var TestObj = new Mock<IUObject>();

        var MacroCommandStrategy = new MacroCommandStrategy();

        ICommand MacroCommand = (ICommand) MacroCommandStrategy.Execute(new object[] {TestStrategyReturnsListOfOperations, TestObj.Object});

        // Act
        // Assert
        MacroCommand.Execute();
    }
}
