namespace SpaceBattle.Lib.Test;

public class GameItemsTests
{
    readonly Dictionary<string, object> scopes = new();
    readonly int quant = 256;
    public GameItemsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var getScopeStrategy = new Mock<IStrategy>();
        getScopeStrategy.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Scope", (object[] args) => getScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create", (object[] args) => new CreateGameStrategy((string) args[0], (int) args[1]).Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(() =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();
        
        scopes.Add("1", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));
    }

    [Fact]
    public void GetItemStrategyTest()
    {
        var obj = new Mock<IUObject>();

        var command = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        command.Execute();

        IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").Add("0", obj.Object);

        var resolvedObj = IoC.Resolve<IUObject>("Get.Item", "0");

        Assert.Equal(obj.Object, resolvedObj);
    }

    [Fact]
    public void DeleteItemStrategyTest()
    {
        var mockObj = new Mock<IUObject>();
        var command = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        command.Execute();

        IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").Add("0", mockObj.Object);
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").Count() == 1);

        IoC.Resolve<ICommand>("Item.Remove", "0").Execute();
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").Count() == 0);
    }

    [Fact]
    public void ItemIsNotExistsTest()
    {
        var mockObj = new Mock<IUObject>();

        var command = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        command.Execute();

        IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").Add("0", mockObj.Object);
        IoC.Resolve<ICommand>("Item.Remove", "0").Execute();
        
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects").Count == 0);
        Assert.Throws<Exception>(() => IoC.Resolve<IUObject>("Get.Item", "0"));
    }
}


