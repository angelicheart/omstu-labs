namespace SpaceBattle.Lib;

public class HardStopThreadCommand : ICommand
{
    private ActionCommand action_after_stop;
    private string id;
    
    public HardStopThreadCommand(string id, ActionCommand action)
    {
        this.action_after_stop = action;
        this.id = id;
    }

    public void Execute()
    {
        ServerThread st = IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", id);

        if(Thread.CurrentThread == st.thread) {
            st.Stop();
            action_after_stop.Execute();
        }
        else {
            throw new Exception();
        }
    }
}
