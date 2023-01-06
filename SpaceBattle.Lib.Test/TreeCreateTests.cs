namespace SpaceBattle.Lib.Test;

public class TreeCreateTest
{
    public TreeCreateTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var TreeStrategy = new TreeCreateStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Tree.Build", (object[] args) => (TreeStrategy.Execute(args))).Execute();
    }

    [Fact]
    public void TreeCreationTest()
    {
        // Arrange
        var path = @"../../../../SpaceBattle.Lib/Data/data.txt";

        var data = File.ReadLines(path);
        var list = data.Select(i => i.Split("; ").Select(int.Parse).ToList()).ToList();
        var tree = IoC.Resolve<IDictionary<int, object>>("Game.Tree.Build", list);

        // Act
        // Assert
        Assert.True(tree.ContainsKey(1));
        Assert.True(tree.ContainsKey(4));
        Assert.True(tree.ContainsKey(7));

        Assert.True(((IDictionary<int, object>) tree[1]).ContainsKey(2));
        Assert.True(((IDictionary<int, object>) tree[1]).ContainsKey(4));
        Assert.True(((IDictionary<int, object>) tree[1]).ContainsKey(8));

        Assert.True(((IDictionary<int, object>) ((IDictionary<int, object>) tree[1])[2]).ContainsKey(3));
    }
}
