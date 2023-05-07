namespace SpaceBattle.Lib;

public class ServerThread
{
    internal ManualResetEvent pauseEvent = new ManualResetEvent(false);

    public void Pause()
    {
        pauseEvent.Reset();
        stop = false;
    }

    public void Resume()
    {
        pauseEvent.Set();
        stop = true;
    }

    internal IReciever reciever;

    internal Thread thread;

    internal bool stop = false;

    internal Action strategy;

    internal ServerThread(IReciever reciever)
    {
        strategy = () => {
            // try {
                HandleCommand();
            // }

            // catch (Exception e) {
            //     IoC.Resolve<IStrategy>("Game.Exceptions.FindExcHandlerForCommands", reciever.Recieve(), e);
            // }   
        };

        this.reciever = reciever;

        thread = new Thread(() => {
             do {
                pauseEvent.WaitOne(Timeout.Infinite);
                strategy();
            } while (!stop);
        });
    }
    
    internal void HandleCommand() 
    {
        var cmd = reciever.Recieve();
        cmd.Execute();
    }

    internal void UpdateBehaviour(Action strategy) {
        this.strategy = strategy;
    }

    internal void Stop()
    {
        stop = true;
    }

    internal void Start()
    {
        pauseEvent.Set();
        thread.Start();
    }
}
