namespace SpaceBattle.Lib.Test;

public class StopThreadCommandsTests
{
    public StopThreadCommandsTests()
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.CreateAndStart", (object[] args) => new CreateAndStartThreadStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.HardStop", (object[] args) => new HardStopThreadStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.SoftStop", (object[] args) => new SoftStopThreadStrategy().Execute(args)).Execute();
    }

    [Fact]
    public void HardStopThreadTest_wAction_succ()
    {
        ManualResetEvent waitHandler = new ManualResetEvent(true);

        var ObjThatMove = new Mock<IMovable>();

        ObjThatMove.SetupGet(m => m.position).Returns(new Vector(12, 5)).Verifiable();
        ObjThatMove.SetupGet(m => m.velocity).Returns(new Vector(-7, 3)).Verifiable();

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Threads.HardStop", "1", new ActionCommand((arg) => {
           new MoveCommand(ObjThatMove.Object).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ExceptionCommand()).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand((arg) => waitHandler.Set())).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        ObjThatMove.VerifySet(m => m.position = new Vector(5, 8));

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }

    [Fact]
    public void HardStopThreadTest_woAction_succ()
    {
        ManualResetEvent waitHandler = new ManualResetEvent(false);

        var ObjThatMove = new Mock<IMovable>();

        ObjThatMove.SetupGet(m => m.position).Returns(new Vector(12, 5)).Verifiable();
        ObjThatMove.SetupGet(m => m.velocity).Returns(new Vector(-7, 3)).Verifiable();

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new MoveCommand(ObjThatMove.Object)).Execute();

        IoC.Resolve<ICommand>("Game.Threads.HardStop", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand((arg) => waitHandler.Set())).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        ObjThatMove.VerifySet(m => m.position = new Vector(5, 8));

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }

    [Fact]
    public void SoftStopThreadTest_wAction_succ()
    {
       AutoResetEvent waitHandler = new AutoResetEvent(true);

        var ObjThatMove = new Mock<IMovable>();

        ObjThatMove.SetupGet(m => m.position).Returns(new Vector(12, 5)).Verifiable();
        ObjThatMove.SetupGet(m => m.velocity).Returns(new Vector(-7, 3)).Verifiable();

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Threads.SoftStop", "1", new ActionCommand((arg) => {
            new MoveCommand(ObjThatMove.Object).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new MoveCommand(ObjThatMove.Object)).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand((arg) => waitHandler.Set())).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        ObjThatMove.VerifySet(m => m.position = new Vector(5, 8));

        Assert.True(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }

    [Fact]
    public void SoftStopThreadTest_woAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(true);

        var ObjThatMove = new Mock<IMovable>();

        ObjThatMove.SetupGet(m => m.position).Returns(new Vector(12, 5)).Verifiable();
        ObjThatMove.SetupGet(m => m.velocity).Returns(new Vector(-7, 3)).Verifiable();

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Threads.SoftStop", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new MoveCommand(ObjThatMove.Object)).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand((arg) => waitHandler.Set())).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        ObjThatMove.VerifySet(m => m.position = new Vector(5, 8));

        Assert.True(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }
}
