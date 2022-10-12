namespace SpaceBattle.Lib.Test;
public class RotateTests
{
    [Fact]
    public void RotatePositive()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Returns(new Angle(Math.PI / 2)).Verifiable();

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(Math.PI / 4).Verifiable();

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        rotateCommand.Execute();

        // Assert
        rotatable.VerifySet(r => r.Direction = new Angle(3 * Math.PI / 4));
    }

    [Fact]
    public void RotateWithoutAngle()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Throws(new Exception());

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(Math.PI / 4).Verifiable();

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }

    [Fact]
    public void RotateWithoutAngleVelocity()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Returns(new Angle(Math.PI / 2)).Verifiable();

        rotatable.SetupGet(r => r.angleVelocty)
            .Throws(new Exception());

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }

    [Fact]
    public void RotateBlocked()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Returns(new Angle(Math.PI / 2)).Verifiable();

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(Math.PI / 4).Verifiable();

        rotatable.SetupSet(r => r.Direction = It.IsAny<Angle>()).Throws(new Exception());

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }
}