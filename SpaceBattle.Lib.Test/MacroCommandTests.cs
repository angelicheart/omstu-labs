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
        var TestObj = new Mock<IUObject>();
        var Commands = new List<ICommand>();

        var StratList = IoC.Resolve<List<string>>(TestStrategyReturnsListOfOperations);

        StratList.ForEach(strat => Commands.Add(IoC.Resolve<ICommand>(strat, TestObj.Object)));

        var MacroCommand = new MacroCommand(Commands);
        MacroCommand.Execute();
    }

    [Fact]
    public void PositiveMacroCommandStrategyTest()
    {   
        var TestObj = new Mock<IUObject>();

        var MacroCommandStrategy = new MacroCommandStrategy();

        ICommand MacroCommand = (ICommand) MacroCommandStrategy.Execute(new object[] {TestStrategyReturnsListOfOperations, TestObj.Object});

        MacroCommand.Execute();
    }
}
