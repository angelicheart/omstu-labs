namespace SpaceBattle.Lib.Test;

public class ServerThreadTests
{
    public ServerThreadTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain", (object[] args) => new ThreadsDomainStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain.Get", (object[] args) => new ServerThreadDomainGetStrategy().Execute(args)).Execute();

    }

    [Fact]
    public void ServerThread_1()
    {
        var queue = new BlockingCollection<ICommand>();

        var reciever = new RecieverAdapter(queue);

        var sender = new SenderAdapter(queue);

        var ObjThatMove = new Mock<IMovable>();

        ObjThatMove.SetupGet(m => m.position).Returns(new Vector(12, 5)).Verifiable();
        ObjThatMove.SetupGet(m => m.velocity).Returns(new Vector(-7, 3)).Verifiable();

        var MoveCommand = new MoveCommand(ObjThatMove.Object);

        sender.Send(MoveCommand);

        ServerThread st = new ServerThread(reciever);

        ConcurrentDictionary<string, ServerThread> ServerThreadDomain = IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Game.Threads.Domain");

        ServerThreadDomain["1"] = st;

        ServerThread test = IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1");

        test.Execute();

        Thread.Sleep(1);

        ObjThatMove.VerifySet(m => m.position = new Vector(5, 8));
    }

    // [Fact]
    // public void ServerThread_test1()
    // {
    //     var queue1 = new BlockingCollection<ICommand>(100);

    //     var reciever = new RecieverAdapter(queue1);

    //     var sender = new Mock<ISender>();
    //     sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

    //     ServerThread st = new ServerThread(reciever);

    //     sender.Object.Send(new ActionCommand(
    //         (arg) => {
    //             new ExceptionCommand();
    //         } 
    //     ));

    //     var queue2 = new BlockingCollection<ICommand>(100);

    //     var reciever2 = new RecieverAdapter(queue1);

    //     var sender2 = new Mock<ISender>();
    //     sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

    //     ServerThread st2 = new ServerThread(reciever);

    //     sender2.Object.Send(new ActionCommand(
    //         (arg) => {
    //             new ExceptionCommand();
    //         } 
    //     ));

    //     st.Execute();
    //     st2.Execute();

    //     Thread.Sleep(1);
    // }
}
