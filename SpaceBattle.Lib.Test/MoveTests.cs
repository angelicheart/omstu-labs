namespace SpaceBattle.Lib.Test;
public class MoveTests
{
    [Fact]
    public void MoveTestPositive()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Returns(new Vector(12, 5)).Verifiable();

        movable.SetupGet(m => m.velocity)
            .Returns(new Vector(-7, 3)).Verifiable();

        var moveCommand = new MoveCommand(movable.Object);

        // Act
        moveCommand.Execute();

        // Assert
        movable.VerifySet(m => m.position = new Vector(5, 8));
    }

    [Fact]
    public void MoveWithoutPosition()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Throws(new Exception());

        movable.SetupGet(m => m.velocity)
            .Returns(new Vector(-7, 3)).Verifiable();

        var moveCommand = new MoveCommand(movable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => moveCommand.Execute());
    }

    [Fact]
    public void MoveWithoutVelocity()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Returns(new Vector(12, 5)).Verifiable();

        movable.SetupGet(m => m.velocity)
            .Throws(new Exception());

        var moveCommand = new MoveCommand(movable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => moveCommand.Execute());
    }

    [Fact]
    public void MoveBlocked()
    {
        // Arrange
        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.position)
            .Returns(new Vector(12, 5)).Verifiable();

        movable.SetupGet(m => m.velocity)
            .Returns(new Vector(-7, 3)).Verifiable();

        movable.SetupSet(m => m.position = It.IsAny<Vector>()).Throws(new Exception());

        var moveCommand = new MoveCommand(movable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => moveCommand.Execute());
    }
}