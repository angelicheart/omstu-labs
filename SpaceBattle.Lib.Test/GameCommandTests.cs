namespace SpaceBattle.Lib.Test;

public class GameCommandTests
{
    public GameCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void GameCommand_succ()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Quantum.Get", (object[] args) => quantumStrategy.Object.Execute()).Execute();

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();

        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));
        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));
        gameQueue.Add(new ActionCommand(() => quantumStrategy.Setup(qs => qs.Execute()).Returns(0)));

        ICommand gameCommand = new GameCommand(scope, gameQueue);

        gameCommand.Execute();

        Assert.True(gameQueue.Count() == 0);
    }
        
    [Fact]
    public void GameCommand_timeExceeded()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Quantum.Get", (object[] args) => quantumStrategy.Object.Execute()).Execute();

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();

        gameQueue.Add(new ActionCommand(() => Task.Delay(1100).Wait()));
        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));

        ICommand gameCommand = new GameCommand(scope, gameQueue);

        gameCommand.Execute();

        Assert.False(gameQueue.Count() == 0);
    }

    [Fact]
    public void ReturnKeyValueHandlerTreeDictTest()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Quantum.Get", (object[] args) => quantumStrategy.Object.Execute()).Execute();

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();

        gameQueue.Add(new ActionCommand(() => Task.Delay(1100).Wait()));
        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));
        gameQueue.Add(new ActionCommand(() => quantumStrategy.Setup(qs => qs.Execute()).Returns(0)));

        ICommand gameCommand = new GameCommand(scope, gameQueue);

        gameCommand.Execute();

        Assert.False(gameQueue.Count() == 0);
    }
}
