namespace SpaceBattle.Lib;

public class CreateCommandStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        IMessage message = (IMessage) args[0];

        var command = IoC.Resolve<ICommand>("Command." + message.CmdType);
        return command;
    }
}
