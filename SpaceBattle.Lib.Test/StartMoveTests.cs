namespace SpaceBattle.Lib.Test;

public class StartMoveCommandTests
{
    public StartMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var strategy = new Mock<IStrategy>();
        strategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(command.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SetProperty", (object[] args) => strategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Event.Move", (object[] args) => strategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => strategy.Object.Execute(args)).Execute();

        var queue = new Mock<IStrategy>();
        queue.Setup(q => q.Execute()).Returns(new Queue<Hwdtech.ICommand>());
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue", (object[] args) => queue.Object.Execute()).Execute();
    }

    [Fact]
    public void StartMoveCommandPositive()
    {
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        StartMoveCommand.Execute();
        MoveStartable.VerifyAll();
    }

    [Fact]
    public void StartMoveCommandVelocityException()
    {
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Throws(new Exception()).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
    }

    [Fact]
    public void StartMoveCommandObjException()
    {
        var MoveStartable = new Mock<IMoveStartable>();
        MoveStartable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStartable.SetupGet(m => m.obj).Throws(new Exception()).Verifiable();

        ICommand StartMoveCommand = new StartMoveCommand(MoveStartable.Object);

        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
    }
}
