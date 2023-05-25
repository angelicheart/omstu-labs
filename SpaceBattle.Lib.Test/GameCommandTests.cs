namespace SpaceBattle.Lib.Test;

public class GameCommandTests
{
    public GameCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
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
    public void GameCommand_exceptionHandledWithDefaultHandler()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Quantum.Get", (object[] args) => quantumStrategy.Object.Execute()).Execute();

        var cess = new CommandExceptionStorageStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ICommand_Exception.Dict.Get", (object[] args) => cess.Execute(args)).Execute();       

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindExceptionHandlerForCmd", (object[] args) => new DefaultExceptionHandlerStrategy().Execute(args)).Execute();

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();

        gameQueue.Add(new ActionCommand(() => throw new Exception()));
        gameQueue.Add(new ActionCommand(() => quantumStrategy.Setup(qs => qs.Execute()).Returns(0)));

        ICommand gameCommand = new GameCommand(scope, gameQueue);

        gameCommand.Execute();

        Assert.True(gameQueue.Count() == 0);
        Assert.Equal(IoC.Resolve<IDictionary<ICommand, Exception>>("Game.ICommand_Exception.Dict.Get").Count(), 1);
    }

    [Fact]
    public void GameCommand_nonResolvedException()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();

        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));

        ICommand gameCommand = new GameCommand(scope, gameQueue);

        Assert.ThrowsAny<Exception>(() => gameCommand.Execute());
    }
}
