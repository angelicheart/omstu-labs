namespace SpaceBattle.Lib;

public class InterpretationTest
{
    Dictionary<int, Queue<ICommand>> currentGames = new Dictionary<int, Queue<ICommand>>();
    public InterpretationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var cmd = new Mock<ICommand>();

        /*
        Реализована команда, которая извлекает не блокируемым образом из очереди сообщение, создает команду интерпретации сообщения и складывает ее в очередь соответствующей игры.
        Команда из пункта 1 вызывается как часть обработки каждой игры, чтобы обеспечить хорошую реакцию сервера на входящие сообщения.
        Команда интерпретации должна создавать команду, соответствующую сообщению и ставить эту команду в очередь игры.
        */

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue", (object[] args) => currentGames).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "PushInQueue", (object[] args) => new InQueueStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue", (object[] args) => new GetQueueStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateCommand", (object[] args) => new CreateCommandStrategy().Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Move", (object[] args) => cmd.Object).Execute();
    }

    [Fact]
    public void SomePositiveTest()
    {
        Mock<IMessage> message = new Mock<IMessage>();

        currentGames.Add(10, new Queue<ICommand>());

        message.SetupGet(x => x.CmdType).Returns("Move");
        message.SetupGet(x => x.GameID).Returns(10);
        message.SetupGet(x => x.ItemID).Returns(0);

        var intcmd = new InterpretationCommand(message.Object);
        intcmd.Execute();
    }
}