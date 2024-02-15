namespace SpaceBattle.Lib.Test;

public class ScopeTests
{
    Dictionary<string, object> scopes = new Dictionary<string, object>();
    int quant = 256;
    public ScopeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var GetScopeStrategy = new Mock<IStrategy>();
        GetScopeStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]); 

        var NewScopeToDictStrategy = new Mock<IStrategy>();
        NewScopeToDictStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) =>
        {
            scopes.Add((string)args[0], IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
            return scopes[(string)args[0]];
        });

        var DeleteScopeFromDictStrategy = new Mock<IStrategy>();
        DeleteScopeFromDictStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => new ActionCommand(() => {
            scopes.Remove((string)args[0]);
        }));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetScope", (object[] args) => GetScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.NewScope", (object[] args) => NewScopeToDictStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteScope", (object[] args) => DeleteScopeFromDictStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNew", (object[] args) => new CreateGameStrategy((string)args[0], (int)args[1]).Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteGame", (object[] args) => new ActionCommand(() => {
            new DeleteGameCommand((string) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(() =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();
    }

    [Fact]
    public void CreateNewGameTest()
    {
        IoC.Resolve<object>("Game.NewScope", "1");

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        CreateGameCommand.Execute();

        var scope = new InitializeScope().Execute("2");

        Assert.True(scopes.Count() == 1);
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);

        Assert.Equal(quant, IoC.Resolve<int>("GetQuantum"));

        Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("QueueDequeue", new Queue<ICommand>()));
    }

    [Fact]
    public void CreateSomeNewGamesTest()
    {
        for (int i = 1; i < 5; i++) 
        {
            IoC.Resolve<object>("Game.NewScope", i.ToString());
            var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", i.ToString(), quant);
            CreateGameCommand.Execute();
        }
        Assert.True(scopes.Count() == 4);
    }

    [Fact]
    public void DeleteGameNegativeTest()
    {
        IoC.Resolve<object>("Game.NewScope", "1");

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        var DeleteGameCommand = IoC.Resolve<ICommand>("Game.DeleteGame", "2");

        CreateGameCommand.Execute();
        DeleteGameCommand.Execute();

        Assert.True(scopes.Count() == 1);
    }

    [Fact]
    public void DeleteGamePositiveTest()
    {
        IoC.Resolve<object>("Game.NewScope", "1");

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        var DeleteGameCommand = IoC.Resolve<ICommand>("Game.DeleteGame", "1");
        DeleteGameCommand.Execute();

        CreateGameCommand.Execute();

        Assert.True(scopes.Count() == 0);
    }
}