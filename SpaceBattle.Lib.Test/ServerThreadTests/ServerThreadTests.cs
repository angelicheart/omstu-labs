namespace SpaceBattle.Lib.Test;
using System.Threading;

public class ServerThreadTests
{
    public ServerThreadTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var tds = new ThreadsDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain", (object[] args) => tds.Execute(args)).Execute();

        var rds = new RecieversDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Recievers.Domain", (object[] args) => rds.Execute(args)).Execute();

        var sds = new SendersDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Domain", (object[] args) => sds.Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain.Set", (object[] args) => new ThreadsDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain.Get", (object[] args) => new ThreadsDomainGetStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Recievers.Domain.Set", (object[] args) => new RecieversDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Recievers.Domain.Get", (object[] args) => new RecieversDomainGetStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Domain.Set", (object[] args) => new SendersDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Domain.Get", (object[] args) => new SendersDomainGetStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Send", (object[] args) => new SendCommandStrategy().Execute(args)).Execute();

    }

    [Fact]
    public void ServerThread_1()
    {
        AutoResetEvent are = new AutoResetEvent(false);

        var queue = new BlockingCollection<ICommand>();

        var reciever = new RecieverAdapter(queue);

        var sender = new SenderAdapter(queue);

        var ObjThatMove = new Mock<IMovable>();

        ObjThatMove.SetupGet(m => m.position).Returns(new Vector(12, 5)).Verifiable();
        ObjThatMove.SetupGet(m => m.velocity).Returns(new Vector(-7, 3)).Verifiable();

        var MoveCommand = new MoveCommand(ObjThatMove.Object);

        var st = new ServerThread(reciever);

        sender.Send(new MoveCommand(ObjThatMove.Object));

        IoC.Resolve<ICommand>("Game.Threads.Domain.Set", "1", st).Execute();
        IoC.Resolve<ICommand>("Game.Recievers.Domain.Set", "1", reciever).Execute();
        IoC.Resolve<ICommand>("Game.Senders.Domain.Set", "1", sender).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Execute();

        Thread.Sleep(3000);

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
