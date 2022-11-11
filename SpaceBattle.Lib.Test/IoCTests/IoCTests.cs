namespace SpaceBattle.Lib.Test;


class SumOfTwoStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        int a = (int) args[0];
        int b = (int) args[1];
        return a + b;
    }
}
public class IoCTest
{
    [Fact]

    public void IoCTest01()
    {
        IoC.resolve<ICommand>("IoC.Add", "Game.UObject.SumOfTwo", new SumOfTwoStrategy()).Execute();
        var res = IoC.resolve<int>("Game.UObject.SumOfTwo", 1, 2);

        Assert.Equal(3, res);
    }
} 