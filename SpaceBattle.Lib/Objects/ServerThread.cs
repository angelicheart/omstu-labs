namespace SpaceBattle.Lib;

public class ServerThread
{
    public ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

    public void Pause()
    {
        _manualResetEvent.Reset();
    }

    public void Resume()
    {
        _manualResetEvent.Set();
    }

    public IReciever reciever;

    public Thread thread;

    public bool stop = false;

    public Action strategy;

    public IStrategy exchandler = IoC.Resolve<IStrategy>("Game.Exception.FindExceptionHandlerForCmd");

    public ServerThread(IReciever reciever)
    {
        strategy = () => {

            var cmd = reciever.Recieve();

            try {
                cmd.Execute();
            }

            catch (Exception e) {
                
                exchandler.Execute(new object[] {cmd, e});
            }   
        };

        this.reciever = reciever;

        thread = new Thread(() => {
             do  {
                _manualResetEvent.WaitOne(Timeout.Infinite);
                
                strategy();
            } while (!stop);
        });
    }

    public void UpdateBehaviour(Action strategy) {
        this.strategy = strategy;
    }

    public void Stop()
    {
        _manualResetEvent.Reset();

        stop = true;
    }

    public void Start()
    {
        thread.Start();
        _manualResetEvent.Set();
    }
}
