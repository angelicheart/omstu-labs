namespace SpaceBattle.Lib.Test;

public class GameItemsTests
{
    Dictionary<string, object> scopes = new Dictionary<string, object>();
    int quant = 256;
    public GameItemsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var GetScopeStrategy = new Mock<IStrategy>();
        GetScopeStrategy.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetScope", (object[] args) => GetScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNew", (object[] args) => new CreateGameStrategy((string)args[0], (int)args[1]).Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(() =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();
        
        scopes.Add("1", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));
    }

    [Fact]
    public void GetItemStrategyPositiveTest()
    {
        var mockObj = new Mock<IUObject>();

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        CreateGameCommand.Execute();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);

        var resolvedObj = IoC.Resolve<IUObject>("General.GetItem", "0");

        Assert.Equal(mockObj.Object, resolvedObj);
    }

    [Fact]
    public void DeleteItemStrategyPositiveTest()
    {
        var mockObj = new Mock<IUObject>();
        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        CreateGameCommand.Execute();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 1);

        IoC.Resolve<ICommand>("General.RemoveItem", "0").Execute();
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);
    }

    [Fact]
    public void ItemIsNotExistsPositiveTest()
    {
        var mockObj = new Mock<IUObject>();

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        CreateGameCommand.Execute();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);
        IoC.Resolve<ICommand>("General.RemoveItem", "0").Execute();
        
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);
        Assert.Throws<Exception>(() => IoC.Resolve<IUObject>("General.GetItem", "0"));
    }
}
