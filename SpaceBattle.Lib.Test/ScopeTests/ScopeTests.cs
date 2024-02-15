namespace SpaceBattle.Lib.Test;

public class ScopeTests
{
    Dictionary<string, object> currentGames = new Dictionary<string, object>();
    public ScopeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var GetScope = new Mock<IStrategy>();
        GetScope.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) => currentGames[(string)args[0]]);

        var NewScopeToDict = new Mock<IStrategy>();
        NewScopeToDict.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) =>
        {
            currentGames.Add((string)args[0], IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
            return currentGames[(string)args[0]];
        });

        var DeleteScopeFromDict = new Mock<IStrategy>();
        DeleteScopeFromDict.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) => new ActionCommand(() => {
            currentGames.Remove((string)args[0]);
        }));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetScope", (object[] args) => GetScope.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.NewScope", (object[] args) => NewScopeToDict.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteScope", (object[] args) => DeleteScopeFromDict.Object.Execute(args)).Execute();
    }

    [Fact]
    public void CreateNewGameTest()
    {
        IoC.Resolve<object>("Game.NewScope", "1");

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNew", (object[] args) => new CreateGameStrategy((string)args[0], (int)args[1]).Execute()).Execute();

        ICommand gameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", 500);
        gameCommand.Execute();

        Assert.True(currentGames.Count() == 1);
        Assert.Equal(500, IoC.Resolve<int>("GetQuantum"));
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);
        Assert.Throws<Exception>(
            () =>
            {
                IoC.Resolve<ICommand>("QueueDequeue", new Queue<ICommand>());
            }
        );
    }

    [Fact]
    public void DeleteGameTest()
    {
        IoC.Resolve<object>("Game.NewScope", "1");

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNew", (object[] args) => new CreateGameStrategy((string)args[0], (int)args[1]).Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteGame", (object[] args) => new DeleteGameStrategy((string)args[0])).Execute();

        ICommand gameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", 500);
        var deleteGame = IoC.Resolve<IStrategy>("Game.DeleteGame", "1");
        gameCommand.Execute();

        deleteGame.Execute();

        Assert.True(currentGames.Count() == 0);
    }
}