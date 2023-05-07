namespace SpaceBattle.Lib;

public class SoftStopThreadCommand : ICommand
{
    private ActionCommand action_after_stop;
    private string id;
    
    public SoftStopThreadCommand(string id, ActionCommand action)
    {
        this.action_after_stop = action;
        this.id = id;
    }

    public void Execute()
    {
        ServerThread st = IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", id);

        while (!st.stop)
        {
            if (!IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", id).isEmpty()) {
                st.strategy();
            }
            else 
            { 
            IoC.Resolve<ICommand>("Game.Senders.Send", id, new ActionCommand((arg) => {
                new StopThreadCommand(st).Execute();
                action_after_stop.Execute();
            })).Execute();

                // IoC.Resolve<ICommand>("Game.Senders.Send", id, new StopThreadCommand(IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", id))).Execute();
                // action_after_stop.Execute();
            }
        }
    }
}
