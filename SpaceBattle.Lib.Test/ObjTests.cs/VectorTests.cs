namespace SpaceBattle.Lib.Test;
public class VectorTests
{
    [Fact]
    public void VectorNullEqual()
    {
        // Arrange
        var a = new Vector(1, 2);

        // Act
        // Assert
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void VectorGetHashCodeTest()
    {
        // Arrange
        var hashcodea = new Vector(1, 2).GetHashCode();
        var hashcodeb = new Vector(1, 2).GetHashCode();
        // Act
        // Assert
        Assert.Equal(hashcodeb, hashcodea);
    }

    [Fact]
    public void SymmetricTest()
    {
        var x = new Vector(1, 2);
        var y = new Vector(3, 4);

        Assert.False(x.Equals(y));
        Assert.False(y.Equals(x));
    }
}