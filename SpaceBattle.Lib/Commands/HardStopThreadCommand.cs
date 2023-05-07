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
        IoC.Resolve<ICommand>("Game.Senders.Send", id, new ActionCommand((arg) => {
            new StopThreadCommand(st).Execute();
            action_after_stop.Execute();
        })).Execute();
        
        // IoC.Resolve<ICommand>("Game.Senders.Send", id, new StopThreadCommand(IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", id))).Execute();
        // action_after_stop.Execute();
    }
}
