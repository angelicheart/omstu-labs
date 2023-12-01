namespace SpaceBattle.Lib;

public class InterpretationCommand : ICommand
{
    private readonly IMessage message;

    public InterpretationCommand(IMessage msg)
    {
        message = msg;
    }

    public void Execute()
    {
        var cmd = IoC.Resolve<ICommand>("CreateCommand", message);

        IoC.Resolve<ICommand>("PushInQueue", message.GameID, cmd).Execute();
    }
}
