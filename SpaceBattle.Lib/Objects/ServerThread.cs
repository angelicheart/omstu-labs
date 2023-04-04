namespace SpaceBattle.Lib;

public class ServerThread
{
    public IReciever reciever;
    Thread thread;
    private bool stop = false;
    private Action strategy; // содержит action который будет выполняться на каждой итерации в потоке
    public ServerThread(IReciever reciever)
    {
        strategy = () => {
                HandleCommand();
        };

        this.reciever = reciever;

        thread = new Thread {
            () => {
                while(!stop) {
                    strategy();
                }
            }
        };
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

    void Execute()
    {
        thread.Start();
    }
}
