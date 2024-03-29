namespace SpaceBattle.Lib.Test;

public class QueueStrategiesTests
{
    readonly Dictionary<string, object> scopes = new();
    readonly int quant = 256;
    
    public QueueStrategiesTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var getScopeStrategy = new Mock<IStrategy>();
        getScopeStrategy.Setup(o => o.Execute(It.IsAny<object[]>())).Returns((object[] args) => scopes[(string)args[0]]);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Scope", (object[] args) => getScopeStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create", (object[] args) => new CreateGameStrategy((string)args[0], (int)args[1]).Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(() =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        scopes.Add("1", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));
    }

    [Fact]
    public void EnqueueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        var command = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        command.Execute();

        IoC.Resolve<ICommand>("Queue.Enqueue", queue, cmd.Object).Execute();
        IoC.Resolve<ICommand>("Queue.Enqueue", queue, cmd.Object).Execute();
        IoC.Resolve<ICommand>("Queue.Enqueue", queue, cmd.Object).Execute();

        Assert.True(queue.Count == 3);
    }

    [Fact]
    public void DequeueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        var command = IoC.Resolve<ICommand>("Game.Create", "1", quant);
        command.Execute();

        IoC.Resolve<ICommand>("Queue.Enqueue", queue, cmd.Object).Execute();

        Assert.Equal(cmd.Object, IoC.Resolve<ICommand>("Queue.Dequeue", queue));
    }
}
