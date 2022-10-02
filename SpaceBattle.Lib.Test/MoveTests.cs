namespace SpaceBattle.Lib.Test;
public class MoveTests
{
    [Fact]
    public void MoveTest1()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Returns(new Vector(12, 5)).Verifiable();

        movable.SetupGet(x => x.velocity)
            .Returns(new Vector(-7, 3)).Verifiable();

        var moveCommand = new MoveCommand(movable.Object);

        // Act
        moveCommand.Execute();

        // Assert
        movable.VerifySet(m => m.position = new Vector(5, 8));
    }

    [Fact]
    public void MoveTest2()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Throws(new Exception());

        movable.SetupGet(x => x.velocity)
            .Returns(new Vector(-7, 3)).Verifiable();
        
        var moveCommand = new MoveCommand(movable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => moveCommand.Execute());
    }

    [Fact]
    public void MoveTest3()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Returns(new Vector(12, 5)).Verifiable();

        movable.SetupGet(x => x.velocity)
            .Throws(new Exception());
        
        var moveCommand = new MoveCommand(movable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => moveCommand.Execute());
    }

    [Fact]
    public void MoveTest4()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Returns(new Vector(12, 5)).Verifiable();

        movable.SetupGet(x => x.velocity)
            .Returns(new Vector(-7, 3)).Verifiable();

        movable.SetupSet(m => m.position = It.IsAny<Vector>()).Throws(new Exception());

        var moveCommand = new MoveCommand(movable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => moveCommand.Execute());
    }
}