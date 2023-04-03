namespace Server;

public interface IReciever {
    ICommand Recieve();
    bool isEmpty();
}

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

        this.reciever = resiever;

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

public class EmptyCommand : ICommand {
    public void Execute () {

    }
}

public class ActionCommand : ICommand {
    private Action<object[]> action;
    private object[] args;

    public ActionCommand(Action action, params object[] args)
    {
        this.action = action;
        this.args = args;
    }
    public Execute() {

    }
}

public interface ICommand {
    void Execute()
    {
        
    }
}

public interface ISender {
    void Send(ICommand command);
}