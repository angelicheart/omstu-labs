namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();
        
        RecieverAdapter reciever = new RecieverAdapter(queue);

        SenderAdapter sender = new SenderAdapter(queue);

        string id = (string)args[0];
        Action action = (Action)args[1];

        IoC.Resolve<ConcurrentDictionary<String, RecieverAdapter>>("Game.Recievers.Domain")[id] = reciever;
        IoC.Resolve<ConcurrentDictionary<String, SenderAdapter>>("Game.Senders.Domain")[id] = sender;

        IoC.Resolve<ConcurrentDictionary<String, ServerThread>>("Game.Threads.Domain")[id] = new ServerThread(IoC.Resolve<ConcurrentDictionary<String, RecieverAdapter>>("Game.Recievers.Domain")[id]);

        
    
    }
}
