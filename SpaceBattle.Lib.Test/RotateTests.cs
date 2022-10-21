namespace SpaceBattle.Lib.Test;
public class RotateTests
{
    [Fact]
    public void RotatePositive()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Returns(new Angle(90)).Verifiable();

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(new Angle(45)).Verifiable();

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        rotateCommand.Execute();

        // Assert
        rotatable.VerifySet(r => r.Direction = new Angle(135));
    }

    [Fact]
    public void RotateWithoutAngle()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Throws(new Exception());

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(new Angle(45)).Verifiable();

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
            .Returns(new Angle(90)).Verifiable();

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
            .Returns(new Angle(90)).Verifiable();

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(new Angle(45)).Verifiable();

        rotatable.SetupSet(r => r.Direction = It.IsAny<Angle>()).Throws(new Exception());

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        // Assert
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }
}