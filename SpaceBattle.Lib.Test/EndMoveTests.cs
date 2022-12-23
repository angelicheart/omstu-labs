namespace SpaceBattle.Lib.Test;
public class EndMoveCommandTests
{
    public EndMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var command = new Mock<SpaceBattle.Lib.ICommand>();
        command.Setup(c => c.Execute());

        var CommandStrategy = new Mock<IStrategy>();
        CommandStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(command.Object);;

        var EmptyStrategy = new Mock<IStrategy>();
        EmptyStrategy.Setup(c => c.Execute()).Returns(command.Object);

        var inject = new Mock<IInjectable>();
        inject.Setup(c => c.Inject(It.IsAny<SpaceBattle.Lib.ICommand>()));

        var InjectableStrategy = new Mock<IStrategy>();
        InjectableStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(inject.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.RemoveProperty", (object[] args) => CommandStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetProperty", (object[] args) => InjectableStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.EmptyCommand", (object[] args) => EmptyStrategy.Object.Execute(args)).Execute();
    }

    [Fact]
    public void EndMoveCommandPositive()
    {
        // Arrange
        var MoveStopable = new Mock<IMoveStopable>();
        MoveStopable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStopable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();
        
        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStopable.SetupGet(m => m.queue).Returns(TestQueue.Object);

        ICommand EndMoveCommand = new EndMoveCommand(MoveStopable.Object);

        // Act
        EndMoveCommand.Execute();

        // Assert
        MoveStopable.VerifyAll();
    }

    [Fact]
    public void EndMoveCommandVelocityException()
    {
        // Arrange
        var MoveStopable = new Mock<IMoveStopable>();
        MoveStopable.SetupGet(m => m.velocity).Throws(new Exception()).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStopable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStopable.SetupGet(m => m.queue).Returns(TestQueue.Object);

        ICommand EndMoveCommand = new EndMoveCommand(MoveStopable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => EndMoveCommand.Execute());
    }

    [Fact]
    public void EndMoveCommandObjException()
    {
        // Arrange
        var MoveStopable = new Mock<IMoveStopable>();
        MoveStopable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStopable.SetupGet(m => m.obj).Throws(new Exception()).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStopable.SetupGet(m => m.queue).Returns(TestQueue.Object);

        ICommand EndMoveCommand = new EndMoveCommand(MoveStopable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => EndMoveCommand.Execute());
    }

    [Fact]
    public void EndMoveCommandQueueException()
    {
        // Arrange
        var MoveStopable = new Mock<IMoveStopable>();
        MoveStopable.SetupGet(m => m.velocity).Returns(new Vector(5, 5)).Verifiable();

        var obj = new Mock<IUObject>();
        MoveStopable.SetupGet(m => m.obj).Returns(obj.Object).Verifiable();

        var TestQueue = new Mock<IQueue<ICommand>>();
        MoveStopable.SetupGet(m => m.queue).Throws(new Exception()).Verifiable();

        ICommand EndMoveCommand = new EndMoveCommand(MoveStopable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => EndMoveCommand.Execute());
    }
}
