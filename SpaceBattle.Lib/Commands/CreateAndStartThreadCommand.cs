namespace SpaceBattle.Lib;

public class CreateAndStartThreadCommand : ICommand
{
    private RecieverAdapter reciever;
    private SenderAdapter sender;
    private ServerThread st;
    private string id;
    
    public CreateAndStartThreadCommand(string id, RecieverAdapter reciever, SenderAdapter sender, ServerThread st)
    {
        this.id = id;
        this.reciever = reciever;
        this.sender = sender;
        this.st = st;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.Recievers.Domain.Set", id, reciever).Execute();
        IoC.Resolve<ICommand>("Game.Senders.Domain.Set", id, sender).Execute();
        IoC.Resolve<ICommand>("Game.Threads.Domain.Set", id, st).Execute();

        IoC.Resolve<ServerThread>("Game.Threads.Domain.Get", id).Start();
    }
}