namespace SpaceBattle.Lib;

public class SendCommandStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string id = (string)args[0];
        ICommand command = (ICommand)args[1];

        return new SendCommand(id, command);
    }
}
