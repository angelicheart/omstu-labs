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
        RecieverAdapter reciever = IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", id); 

        Action SoftStop = () => {
            while (!st.stop)
                if (!reciever.isEmpty()) {
                    st.strategy();
                }
                else 
                { 
                    st.Stop();
                    action_after_stop.Execute();
                }
            };

        IoC.Resolve<ICommand>("Game.Senders.Send", id, new ActionCommand((arg) => {
            SoftStop();
        })).Execute();
    }
}
