namespace SpaceBattle.Lib;

class MyThread
{
    Thread thread;

    IReciever queue;
    bool stop = false;
    Action strategy;

    internal void Stop() => stop = true;

    internal void HandleCommand()
    {
        var cmd = queue.Recieve();

        cmd.Execute();
    }
    public MyThread(IReciever queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            HandleCommand();
        };

        thread = new Thread(() =>
        {
            while (!stop)
                strategy();
        });
    }

    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }

    public void Execute()
    {
        thread.Start();
    }
}
