namespace SpaceBattle.Lib;

public class InterpretationTest
{
    readonly Dictionary<int, Queue<ICommand>> currentGames = new Dictionary<int, Queue<ICommand>>();
    public InterpretationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        currentGames.Add(1, new Queue<ICommand>());
        currentGames.Add(2, new Queue<ICommand>());
        currentGames.Add(3, new Queue<ICommand>());

        var command = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CommandName", (object[] args) => command.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue", (object[] args) => currentGames).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateCommand", (object[] args) => new CreateCommandStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "PushInQueue", (object[] args) => new InQueueStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue", (object[] args) => new GetQueueStrategy().Execute(args)).Execute();
    }

    [Fact]
    public void PositiveTest()
    {
        var message = new Mock<IMessage>();

        var jsonData = new
        {
            CommandType = "CommandName",
            GameID = 1,
            ItemID = 1
        };

        message.SetupGet(x => x.CmdType).Returns(jsonData.CommandType);
        message.SetupGet(x => x.GameID).Returns(jsonData.GameID);
        message.SetupGet(x => x.ItemID).Returns(jsonData.ItemID);

        var intcmd = new InterpretationCommand(message.Object);
        intcmd.Execute();

        jsonData = new
        {
            CommandType = "CommandName",
            GameID = 3,
            ItemID = 1
        };

        message.SetupGet(x => x.CmdType).Returns(jsonData.CommandType);
        message.SetupGet(x => x.GameID).Returns(jsonData.GameID);
        message.SetupGet(x => x.ItemID).Returns(jsonData.ItemID);

        intcmd = new InterpretationCommand(message.Object);
        intcmd.Execute();
        intcmd.Execute();

        Assert.True(currentGames[1].Count == 1);
        Assert.True(currentGames[2].Count == 0);
        Assert.True(currentGames[3].Count == 2);
    }

    [Fact]
    public void GameIDException()
    {
        var message = new Mock<IMessage>();

        var jsonData = new
        {
            CommandType = "CommandName",
            GameID = 4,
            ItemID = 1
        };

        message.SetupGet(x => x.CmdType).Returns(jsonData.CommandType);
        message.SetupGet(x => x.GameID).Returns(jsonData.GameID);
        message.SetupGet(x => x.ItemID).Returns(jsonData.ItemID);

        var intcmd = new InterpretationCommand(message.Object);

        Assert.ThrowsAny<Exception>(() => intcmd.Execute());
    }

    [Fact]
    public void CommandTypeException()
    {
        var message = new Mock<IMessage>();

        var jsonData = new
        {
            CommandType = "NonExistentCommand",
            GameID = 2,
            ItemID = 1
        };

        message.SetupGet(x => x.CmdType).Returns(jsonData.CommandType);
        message.SetupGet(x => x.GameID).Returns(jsonData.GameID);
        message.SetupGet(x => x.ItemID).Returns(jsonData.ItemID);

        var intcmd = new InterpretationCommand(message.Object);

        Assert.ThrowsAny<Exception>(() => intcmd.Execute());
    }
}
