namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();

        string id = (string)args[0];
        Action action = (Action)args[1];

        RecieverAdapter reciever = new RecieverAdapter(queue);

        ActionCommand ac = new ActionCommand(
            (arg) => {

            } 
        );

        ac.Execute();
    
        ServerThread st = new ServerThread(reciever);

        return st;
    }
}
