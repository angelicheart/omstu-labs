namespace SpaceBattle.Lib;

public class InQueueStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        int id = (int) args[0];
        ICommand cmd = (ICommand) args[1];

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>('вернуть очередь', id);
        return new ActionCommand(() => { queue.Enqueue(cmd); });
    }
}