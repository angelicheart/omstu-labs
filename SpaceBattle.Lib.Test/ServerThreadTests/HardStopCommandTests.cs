namespace SpaceBattle.Lib.Test;

public class HardStopCommandTests
{
    public HardStopCommandTests()
    {
        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 50)]
    public void HardStopThreadTest_wAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();
        
        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ICommand>("Game.Threads.HardStop", "1", new ActionCommand((arg) => {
            new EmptyCommand().Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ExceptionCommand()).Execute();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }

    [Fact(Timeout = 50)]
    public void HardStopThreadTest_woAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ICommand>("Game.Threads.HardStop", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ExceptionCommand()).Execute();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }
}
