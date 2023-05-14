namespace SpaceBattle.Lib.Test;

public class ExceptionHandledFromThreadTests
{
    object scope;
    public ExceptionHandledFromThreadTests() {
        new InitScopeBasedIoCImplementationCommand().Execute(); 

        scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 100)]
    public void ExceptionHandledFromThreadTest()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand(() => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            throw new Exception();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", IoC.Resolve<ICommand>("Game.Threads.HardStop", "1")).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            throw new Exception();
        })).Execute();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<IReceiver>("Game.Receivers.Domain.Get", "1").isEmpty());
    }

    [Fact(Timeout = 100)]
    public void ExceptionHandledFromThreadActionTest()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand(() => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", waitHandlerSet).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new ActionCommand(() => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        })).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", IoC.Resolve<ICommand>("Game.Threads.HardStop", "1", new ActionCommand(() => {
            throw new Exception();
        }))).Execute();

        IoC.Resolve<ICommand>("Game.Senders.Send", "1", new EmptyCommand()).Execute();
        
        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<IReceiver>("Game.Receivers.Domain.Get", "1").isEmpty());
    }
}
