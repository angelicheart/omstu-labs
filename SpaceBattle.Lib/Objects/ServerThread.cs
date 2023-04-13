namespace SpaceBattle.Lib;

public class ServerThread
{
    public RecieverAdapter reciever;

    public Thread thread;

    private bool stop = false;

    private Action strategy;

    public ServerThread(RecieverAdapter reciever)
    {
        strategy = () => {
            HandleCommand();
        };

        this.reciever = reciever;

        thread = new Thread(() =>
        {
            while (!stop ^ !reciever.isEmpty())
            {
                strategy();
            }
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

    public void Execute()
    {
        thread.Start();
    }
}
