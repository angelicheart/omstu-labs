namespace SpaceBattle.Lib;

public class SendCommand : ICommand
{
    private ICommand command;
    private string id;
    
    public SendCommand(string id, ICommand command)
    {
        this.command = command;
        this.id = id;
    }

    public void Execute()
    {
        IoC.Resolve<ISender>("Game.Senders.Domain.Get", id).Send(command);
    }
}
