namespace SpaceBattle.Lib;

public class StopThreadCommand : ICommand
{
    private ServerThread thread;
    
    public StopThreadCommand(ServerThread thread)
    {
        this.thread = thread;
    }

    public void Execute()
    {
        thread.Stop();
    }
}
