namespace SpaceBattle.Lib.Test;

public class ScopeTests
{
    readonly Dictionary<string, object> scopes = new();
    readonly int quant = 256;
    public ScopeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var getScopeStrategy = new Mock<IStrategy>();
        getScopeStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]); 

        var newScopeStrategy = new Mock<IStrategy>();
        newScopeStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) =>
        {
            scopes.Add((string)args[0], IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
            return scopes[(string)args[0]];
        });

        var deleteScopeStrategy = new Mock<IStrategy>();
        deleteScopeStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => new ActionCommand(() => {
            scopes.Remove((string)args[0]);
        }));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Scope", (object[] args) => getScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Create", (object[] args) => newScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Delete", (object[] args) => deleteScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create", (object[] args) => new CreateGameStrategy((string)args[0], (int)args[1]).Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete", (object[] args) => new ActionCommand(() => {
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
        IoC.Resolve<object>("Scope.Create", "1");

        ICommand command = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        command.Execute();

        Dictionary<string, IUObject> objects = IoC.Resolve<Dictionary<string, IUObject>>("Get.Objects");

        Assert.True(scopes.Count == 1);
        Assert.True(objects.Count == 0);
        Assert.Equal(quant, IoC.Resolve<int>("Get.Quantum"));
        Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Queue.Dequeue", new Queue<ICommand>()));
    }

    [Fact]
    public void CreateSomeNewGamesTest()
    {
        for (int i = 1; i < 5; i++) 
        {
            IoC.Resolve<object>("Scope.Create", i.ToString());
            ICommand command = IoC.Resolve<ICommand>("Game.Create", i.ToString(), quant);
            command.Execute();
        }
        Assert.True(scopes.Count == 4);
    }

    [Fact]
    public void DeleteUnexistGameTest()
    {
        IoC.Resolve<object>("Scope.Create", "1");

        ICommand createCommand = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        ICommand deleteCommand = IoC.Resolve<ICommand>("Game.Delete", "2");

        createCommand.Execute();
        deleteCommand.Execute();

        Assert.True(scopes.Count == 1);
    }

    [Fact]
    public void DeleteExistGameTest()
    {
        IoC.Resolve<object>("Scope.Create", "1");

        ICommand createCommand = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        ICommand deleteCommand = IoC.Resolve<ICommand>("Game.Delete", "1");

        deleteCommand.Execute();
        createCommand.Execute();

        Assert.True(scopes.Count == 0);
    }
}
