namespace SpaceBattle.Lib.Test;

public class ExceptionHandledFromThreadTests
{
    public ExceptionHandledFromThreadTests()
    {
        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 100)]
    public void ExceptionHandledFromThreadTest()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ExceptionCommand()).Execute();

        IoC.Resolve<ICommand>("Game.Threads.HardStop", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ExceptionCommand()).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }

    [Fact(Timeout = 100)]
    public void ExceptionHandledFromThreadActionTest()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(true);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Threads.HardStop", "1", new ActionCommand((arg) => {
            new ExceptionCommand().Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ExceptionCommand()).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }
}
