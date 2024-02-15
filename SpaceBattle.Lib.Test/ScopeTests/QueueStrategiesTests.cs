namespace SpaceBattle.Lib.Test;

public class QueueStrategiesTests
{
    Dictionary<string, object> scopes = new Dictionary<string, object>();
    public QueueStrategiesTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        var GetScope = new Mock<IStrategy>();
        GetScope.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetScope", (object[] args) => GetScope.Object.Execute(args)).Execute();
        scopes.Add("1", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));

    }

    [Fact]
    public void enqueueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        ICommand gameCommand = (ICommand)new CreateGameStrategy("1").Execute();
        gameCommand.Execute();

        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();
        Assert.True(queue.Count() == 1);
    }

    [Fact]
    public void dequeueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        ICommand gameCommand = (ICommand)new CreateGameStrategy("1").Execute();
        gameCommand.Execute();

        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();

        var cmd1 = IoC.Resolve<ICommand>("QueueDequeue", queue);
        Assert.Equal(cmd.Object, cmd1);
    }
}