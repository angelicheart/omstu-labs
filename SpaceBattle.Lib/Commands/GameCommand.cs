namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    object scope;
    IReceiver receiver;
    Stopwatch stopwatch = new Stopwatch();
    
    public GameCommand(object scope, IReceiver receiver)
    {
        this.scope = scope;
        this.receiver = receiver;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Int32 GameTime = IoC.Resolve<int>("Game.Quantum.Get");

        stopwatch.Start();

        while(stopwatch.ElapsedMilliseconds <= GameTime && !receiver.isEmpty())
        {
            var cmd = receiver.Receive();
                
            try {
                cmd.Execute();
            }

            catch (Exception e) {
                IoC.Resolve<ICommand>("Game.Exception.FindExceptionHandlerForCmd", cmd, e).Execute();
            }
        }

        stopwatch.Stop();
    }
}
