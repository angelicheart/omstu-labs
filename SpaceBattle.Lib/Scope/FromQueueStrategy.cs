namespace SpaceBattle.Lib;


public class FromQueueStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        Queue<ICommand> queue = (Queue<ICommand>) args[0];
        queue.TryDequeue(out ICommand command);

        if (command != null) return command;
        throw new Exception();
    }
}
