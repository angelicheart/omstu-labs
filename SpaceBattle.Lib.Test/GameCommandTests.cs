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
        ReceiverAdapter receiver = new ReceiverAdapter(gameQueue);

        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));
        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));

        ICommand gameCommand = new GameCommand(scope, receiver);

        gameCommand.Execute();

        Assert.True(receiver.isEmpty());
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
        ReceiverAdapter receiver = new ReceiverAdapter(gameQueue);

        gameQueue.Add(new ActionCommand(() => Task.Delay(1100).Wait()));
        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));

        ICommand gameCommand = new GameCommand(scope, receiver);

        gameCommand.Execute();

        Assert.False(receiver.isEmpty());
    }

    [Fact]
    public void GameCommand_exceptionHandledWithDefaultHandler()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var DefaultExceptionHandler = new DefaultExceptionHandlerStrategy();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindExceptionHandlerForCmd", (object[] args) => DefaultExceptionHandler.Execute(args)).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Quantum.Get", (object[] args) => quantumStrategy.Object.Execute()).Execute();

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();
        ReceiverAdapter receiver = new ReceiverAdapter(gameQueue);

        gameQueue.Add(new ActionCommand(() => throw new Exception()));
        gameQueue.Add(new ActionCommand(() => throw new Exception()));

        ICommand gameCommand = new GameCommand(scope, receiver);

        Assert.ThrowsAny<Exception>(() => gameCommand.Execute());
    }

    [Fact]
    public void GameCommand_nonResolvedException()
    {
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var quantumStrategy = new Mock<IStrategy>();
        quantumStrategy.Setup(qs => qs.Execute()).Returns(1000);

        BlockingCollection<ICommand> gameQueue = new BlockingCollection<ICommand>();
        ReceiverAdapter receiver = new ReceiverAdapter(gameQueue);

        gameQueue.Add(new ActionCommand(() => new EmptyCommand().Execute()));

        ICommand gameCommand = new GameCommand(scope, receiver);

        Assert.ThrowsAny<Exception>(() => gameCommand.Execute());
    }
}
