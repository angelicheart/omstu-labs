namespace SpaceBattle.Lib.Test;

public class SoftStopCommandTests
{
    public SoftStopCommandTests()
    {
        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 100)]
    public void SoftStopThreadTest_wAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Threads.SoftStop", "1", new ActionCommand((arg) => {
            new EmptyCommand().Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        Assert.True(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }

    [Fact(Timeout = 100)]
    public void SoftStopThreadTest_woAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Pause();

        IoC.Resolve<ICommand>("Game.Threads.SoftStop", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();
   
        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Resume();

        waitHandler.WaitOne();

        Assert.True(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }
}
