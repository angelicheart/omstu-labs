namespace SpaceBattle.Lib;

public class ServerThread
{
    private IReceiver receiver;

    public Thread thread { get; }

    public bool stop = false; 

    private Action strategy;

    public ServerThread(IReceiver receiver)
    {
        strategy = () => {
            HandleCommand();
        };

        this.receiver = receiver;

        thread = new Thread(() => {
            while (!stop) {
                strategy();
            }
        });
    }

    public void HandleCommand() {
        var cmd = receiver.Receive();
        try {
            cmd.Execute();
        }
        catch (Exception e) {
            IoC.Resolve<IStrategy>("Game.Exception.FindExceptionHandlerForCmd").Execute(cmd, e);
        }   
    }

    public void UpdateBehaviour(Action strategy) {
        this.strategy = strategy;
    }

    public void Stop()
    {
        stop = true;
    }

    public void Start()
    {
        thread.Start();
    }
}
