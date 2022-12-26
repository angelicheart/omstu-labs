namespace SpaceBattle.Lib.Test;

public class CollisionCheckCommandTests
{
    public CollisionCheckCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var StrategyReturnsList = new Mock<IStrategy>();
        StrategyReturnsList.Setup(srl => srl.Execute(It.IsAny<object[]>())).Returns(new List<int>());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.GetList", (object[] args) => StrategyReturnsList.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.GetDeltas", (object[] args) => new GetDeltasStrategy().Execute(args)).Execute();
    }
    
    [Fact]
    public void CollisionCheckTrue()
    {
        // Arrange
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        var CheckReturnsTrue = new Mock<IStrategy>();
        CheckReturnsTrue.Setup(crt => crt.Execute(It.IsAny<object[]>())).Returns((object) true);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.CheckWithTree", (object[] args) => CheckReturnsTrue.Object.Execute(args)).Execute();

        ICommand CollisionCheckCommand = new CollisionCheckCommand(obj1.Object, obj2.Object);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => CollisionCheckCommand.Execute());
    }

    [Fact]
    public void CollisionCheckFalse()
    {   
        // Arrange
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        var CheckReturnsFalse = new Mock<IStrategy>();
        CheckReturnsFalse.Setup(crf => crf.Execute(It.IsAny<object[]>())).Returns((object) false);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.CheckWithTree", (object[] args) => CheckReturnsFalse.Object.Execute(args)).Execute();

        ICommand CollisionCheckCommand = new CollisionCheckCommand(obj1.Object, obj2.Object);

        // Act
        // Assert
        CollisionCheckCommand.Execute();
    }

    [Fact]
    public void CollisionCheckNull()
    {
        // Arrange
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        var CheckReturnsNull = new Mock<IStrategy>();
        CheckReturnsNull.Setup(crn => crn.Execute(It.IsAny<object[]>())).Throws(new NullReferenceException());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.CheckWithTree", (object[] args) => CheckReturnsNull.Object.Execute(args)).Execute();

        ICommand CollisionCheckCommand = new CollisionCheckCommand(obj1.Object, obj2.Object);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => CollisionCheckCommand.Execute());
    }
}
