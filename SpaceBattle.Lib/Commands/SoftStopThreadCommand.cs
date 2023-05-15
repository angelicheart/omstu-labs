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
        ReceiverAdapter Receiver = IoC.Resolve<ReceiverAdapter>("Game.Receivers.Domain.Get", id); 

        Action SoftStop = () => {
            while (!st.stop) {
                if (!Receiver.isEmpty()) {
                    st.HandleCommand();
                 }
                else { 
                    st.Stop();
                    action_after_stop.Execute();
                }
            }
        };

        if(Thread.CurrentThread == st.thread) {
            st.UpdateBehaviour(SoftStop);
        }
        else {
            throw new Exception();
        }
    }   
}
