namespace SpaceBattle.Lib.Test;
public class AngleTests
{
    [Fact]
    public void AngleNullEqual()
    {
        // Arrange
        var a = new Angle(1, 2);

        // Act
        // Assert
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void VectorGetHashCodeTest()
    {
        // Arrange
        var hashcodea = new Angle(1, 2).GetHashCode();
        var hashcodeb = new Angle(1, 2).GetHashCode();

        // Act
        // Assert
        Assert.Equal(hashcodeb, hashcodea);
    }

    [Fact]
    public void SymmetricTest()
    {
        var x = new Angle(1, 2);
        var y = new Angle(3, 4);

        Assert.False(x.Equals(y));
        Assert.False(y.Equals(x));
    }

    [Fact]
    public void DecimaToFraction1()
    {
        // Arrange
        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(r => r.Direction)
            .Returns(new Angle(20000, 17)).Verifiable();

        rotatable.SetupGet(r => r.angleVelocty)
            .Returns(new Angle(0)).Verifiable();

        var rotateCommand = new RotateCommand(rotatable.Object);

        // Act
        rotateCommand.Execute();

        // Assert
        rotatable.VerifySet(r => r.Direction = new Angle(1640, 17));
    }
}