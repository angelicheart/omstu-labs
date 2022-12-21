namespace SpaceBattle.Lib.Test;

public class CheckCollisionCommandTests
{
    public CheckCollisionCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.Tree", (object[] args) => new Mock<IDictionary<double, object>>().Object).Execute();
    }

    [Fact]
    public void CollisionCheckTrue()
    {
        // Arrange
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.CheckWithTree", (object[] args) => (object) true).Execute();

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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.CheckWithTree", (object[] args) => (object) false).Execute();

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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision.CheckWithTree", (object[] args) => new NullReferenceException()).Execute();

        ICommand CollisionCheckCommand = new CollisionCheckCommand(obj1.Object, null);

        // Act
        // Assert
        Assert.ThrowsAny<Exception>(() => CollisionCheckCommand.Execute());
    }
}