namespace SpaceBattle.Lib.Test;

public class GameTests
{
    Dictionary<int, object> currentGames = new Dictionary<int, object>();
    public GameTests() 
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var getGameStrategy = new Mock<IStrategy>();
        getGameStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => currentGames[(int) args[0]]);

        var addGameStrategy = new Mock<IStrategy>();
        addGameStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => 
        {
            currentGames.Add((int) args[0], IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
            return currentGames[(int) args[0]];
        });

        var removeGameStrategy = new Mock<IStrategy>();
        removeGameStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns((object[] args) => new ActionCommand(() =>
        {
            currentGames.Remove((int) args[0]);
        }));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AddGame", (object[] args) => addGameStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGame", (object[] args) => getGameStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RemoveGame", (object[] args) => removeGameStrategy.Object.Execute(args)).Execute();
    }

    [Fact]
    public void AddGameTest()
    {
        IoC.Resolve<object>("AddGame", 1);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand", (object[] args) => 
        new ActionCommand(() =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", (string) args[0]).Execute();
        })).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateScope", (object[] args) =>
        new CreateGame((int) args[0], (int) args[1]).Execute()).Execute();

        ICommand cmd = IoC.Resolve<ICommand>("CreateScope", 1, 500);
        cmd.Execute();

        Assert.True(currentGames.Count() == 1);
        //Assert.Equal(500, IoC.Resolve<int>("GetQuant"));
        //Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);
        //Assert.Throws<Exception>(() => 
        //{
        //    IoC.Resolve<ICommand>("QueueDequeue", new Queue<ICommand>());
        //});
    }
}
