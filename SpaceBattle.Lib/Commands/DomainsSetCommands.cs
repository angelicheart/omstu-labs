namespace SpaceBattle.Lib;

public class RecieversDomainSetCommand : ICommand
{
    private string id;
    private RecieverAdapter reciever;
    public RecieversDomainSetCommand(string id, RecieverAdapter reciever)
    {   
        this.id = id;
        this.reciever = reciever;
    }
    public void Execute()
    {
        IoC.Resolve<ConcurrentDictionary<string, RecieverAdapter>>("Game.Recievers.Domain", "1")[id] = reciever;
    }
} 

public class ThreadsDomainSetCommand : ICommand
{
    private string id;
    private ServerThread thread;
    public ThreadsDomainSetCommand(string id, ServerThread thread)
    {   
        this.id = id;
        this.thread = thread;
    }
    public void Execute()
    {
        IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Game.Threads.Domain", "1")[id] = thread;
    }
} 

public class SendersDomainSetCommand : ICommand
{
    private string id;
    private SenderAdapter sender;
    public SendersDomainSetCommand(string id, SenderAdapter sender)
    {   
        this.id = id;
        this.sender = sender;
    }
    public void Execute()
    {
        IoC.Resolve<ConcurrentDictionary<string, SenderAdapter>>("Game.Senders.Domain", "1")[id] = sender;
    }
} 
