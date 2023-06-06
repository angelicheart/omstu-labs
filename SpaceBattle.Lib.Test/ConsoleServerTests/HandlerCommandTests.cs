namespace SpaceBattle.Lib.Test;

public class HandlerCommandTests
{
    const int n_threads = 3;

    public HandlerCommandTests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var start = new Mock<SpaceBattle.Lib.ICommand>();
        start.Setup(c => c.Execute());

        var StartThreadStrategy = new Mock<IStrategy>();
        StartThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Throws(new Exception());

        var send = new Mock<SpaceBattle.Lib.ICommand>();
        send.Setup(c => c.Execute());

        var SendStrategy = new Mock<IStrategy>();
        SendStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Throws(new Exception());

        var stop = new Mock<SpaceBattle.Lib.ICommand>();
        stop.Setup(c => c.Execute());

        var StopThreadStrategy = new Mock<IStrategy>();
        StopThreadStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(stop.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.CreateAndStart", (object[] args) => StartThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.SoftStop", (object[] args) => StopThreadStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Send", (object[] args) => SendStrategy.Object.Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartServerCommand", (object[] args) => new ActionCommand(() => {
            new StartServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => new ActionCommand(() => {
            new StopServerCommand((int) args[0]).Execute();
        })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CatchException", (object[] args) => new ActionCommand(() => {
            new HandlerCommand((string) args[0]).Execute();
        })).Execute();
    }

    [Fact]
    public void HandlerCommandTest()
    {
        var cmd = new HandlerCommand("Exception");
        cmd.Execute();
        var str = File.ReadLines("Exceptions.txt").ToList().Last();
        Assert.Equal("Exception", str);
    }

    [Fact]
    public void StartServerExceptionTest()
    {
        IoC.Resolve<ICommand>("StartServerCommand", n_threads).Execute();

        var str = File.ReadLines("Exceptions.txt").ToList().Last();
        Assert.Equal("Start Thread, Exception of type 'System.Exception' was thrown.", str);
    }

    [Fact]
    public void StopServerExceptionTest()
    {
        IoC.Resolve<ICommand>("StopServerCommand", n_threads).Execute();
        
        var str = File.ReadLines("Exceptions.txt").ToList().Last();
        Assert.Equal("Soft Stop Thread, Exception of type 'System.Exception' was thrown.", str);
    }
}
