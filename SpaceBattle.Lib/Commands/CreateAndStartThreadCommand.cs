namespace SpaceBattle.Lib;

public class CreateAndStartThreadCommand : ICommand
{
    private ReceiverAdapter Receiver;
    private SenderAdapter sender;
    private ServerThread st;
    private string id;
    
    public CreateAndStartThreadCommand(string id, ReceiverAdapter Receiver, SenderAdapter sender, ServerThread st)
    {
        this.id = id;
        this.Receiver = Receiver;
        this.sender = sender;
        this.st = st;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.Receivers.Domain.Set", id, Receiver).Execute();
        IoC.Resolve<ICommand>("Game.Senders.Domain.Set", id, sender).Execute();
        IoC.Resolve<ICommand>("Game.Threads.Domain.Set", id, st).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", id).Start();
    }
}
