namespace SpaceBattle.Lib.Test;

public class QueueStrategiesTests
{
    Dictionary<string, object> scopes = new Dictionary<string, object>();
    
    int quant = 256;
    public QueueStrategiesTests()
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
    public void EnqueuePositiveTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        CreateGameCommand.Execute();

        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();
        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();
        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();

        Assert.True(queue.Count() == 3);
    }

    [Fact]
    public void DequeuePositiveTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        var CreateGameCommand = IoC.Resolve<ICommand>("Game.CreateNew", "1", quant);
        CreateGameCommand.Execute();

        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();

        Assert.Equal(cmd.Object, IoC.Resolve<ICommand>("QueueDequeue", queue));
    }
}
