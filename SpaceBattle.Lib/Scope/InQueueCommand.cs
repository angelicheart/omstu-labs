namespace SpaceBattle.Lib;


public class InQueueCommand : ICommand
{
    Queue<ICommand> target;
    ICommand command;
    public InQueueCommand(Queue<ICommand> _target, ICommand cmd)
    {
        target = _target;
        command = cmd;
    }
    public void Execute()
    {
        target.Enqueue(command);
    }
}