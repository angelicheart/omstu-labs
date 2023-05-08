namespace SpaceBattle.Lib.Test;

public class UpdateBehaviourTests
{
    public UpdateBehaviourTests()
    {
        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 50)]
    public void UpdateBehaviour_succ()
    {
        AutoResetEvent waitHandler = new AutoResetEvent(false);
        ActionCommand waitHandlerSet = new ActionCommand((arg) => {
            waitHandler.Set();
        });

        IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", "1").Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").UpdateBehaviour(() => {
            waitHandlerSet.Execute();
        });

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", "1").Stop();

        waitHandler.WaitOne();

        Assert.False(IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").isEmpty());
    }
}