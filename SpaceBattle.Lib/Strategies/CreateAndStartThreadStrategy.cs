namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();

        ActionCommand action;

        if (args.Length == 2) 
        action = (ActionCommand) args[1];
         
        else
        {
            action = new ActionCommand((arg) => {new EmptyCommand();});
        }

        queue.Add(action);
        
        RecieverAdapter reciever = new RecieverAdapter(queue);
        SenderAdapter sender = new SenderAdapter(queue);
        ServerThread st = new ServerThread(reciever);

        string id = (string)args[0];

        return new CreateAndStartThreadCommand(id, reciever, sender, st);
    }
}
