namespace SpaceBattle.Lib;

public class ReceiversDomainSetCommand : ICommand
{
    private string id;
    private ReceiverAdapter Receiver;
    public ReceiversDomainSetCommand(string id, ReceiverAdapter Receiver)
    {   
        this.id = id;
        this.Receiver = Receiver;
    }
    public void Execute()
    {
        IoC.Resolve<ConcurrentDictionary<string, ReceiverAdapter>>("Game.Receivers.Domain")[id] = Receiver;
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
        IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Game.Threads.Domain")[id] = thread;
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
        IoC.Resolve<ConcurrentDictionary<string, SenderAdapter>>("Game.Senders.Domain")[id] = sender;
    }
} 
