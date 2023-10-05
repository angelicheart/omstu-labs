namespace SpaceBattle.Lib;

public class InterpretationCommand : ICommand
{
    IMessage message;

    public InterpretationCommand(IMessage msg)
    {
        message = msg;
    }

    public void Execute()
    {
        // создание команды из сообщения

        // закидывание команды в очередь
    }
}