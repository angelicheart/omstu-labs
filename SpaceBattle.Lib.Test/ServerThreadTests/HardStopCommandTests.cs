namespace SpaceBattle.Lib.Test;

public class HardStopCommandTests
{
    object scope;
    public HardStopCommandTests() {
        new InitScopeBasedIoCImplementationCommand().Execute(); 

        scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 100)]
    public void HardStopThreadTest_wAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand(() => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", IoC.Resolve<ICommand>("Game.Threads.HardStop", "1", new ActionCommand(() => {
            new EmptyCommand().Execute();
        }))).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<IReceiver>("Game.Receivers.Domain.Get", "1").isEmpty());
    }

    [Fact(Timeout = 100)]
    public void HardStopThreadTest_woAction_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand(() => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", IoC.Resolve<ICommand>("Game.Threads.HardStop", "1")).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<IReceiver>("Game.Receivers.Domain.Get", "1").isEmpty());
    }

    [Fact(Timeout = 100)]
    public void HardStopThreadTest_FromOtherThread()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand(() => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "2").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "2", new ActionCommand(() => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "2", IoC.Resolve<ICommand>("Game.Threads.HardStop", "1")).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "2", IoC.Resolve<ICommand>("Game.Threads.HardStop", "2")).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", IoC.Resolve<ICommand>("Game.Threads.HardStop", "1")).Execute();
    }
}
