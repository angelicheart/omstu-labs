namespace SpaceBattle.Lib.Test;

public class StartMoveCommandTests
{
    static StartMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(command.Object);

        var MovableStrategy = new Mock<IStrategy>();
        MovableStrategy.Setup(m => m.Execute(It.IsAny<object[]>())).Returns((new Mock<IMovable>()).Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapters.Movable", (object[] args) => MovableStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Event.Move", (object[] args) => CommandStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SetProperty", (object[] args) => new SetPropertyStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new QueuePushStrategy().Execute(args)).Execute();
    }

    [Fact]
    public void StartMoveCommandPositive()
    {   
        // Arrange
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStartable.SetupGet(m => m.queue).Returns(TestQueue.Object);

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        // Act
        StartMoveCommand.Execute();

        // Assert
        MoveStartable.VerifyAll();
    }

    [Fact]
    public void StartMoveCommandVelocityException()
    {   
        // Arrange
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Throws(new Exception()).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStartable.SetupGet(m => m.queue).Returns(TestQueue.Object);

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
    }

    [Fact]
    public void StartMoveCommandObjException()
    {
        // Arrange
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Throws(new Exception()).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStartable.SetupGet(m => m.queue).Returns(TestQueue.Object);

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
    }

    [Fact]
    public void StartMoveCommandQueueException()
    {
        // Arrange
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStartable.SetupGet(m => m.queue).Throws(new Exception()).Verifiable();

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
    }
}
