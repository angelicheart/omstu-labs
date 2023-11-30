namespace SpaceBattle.Lib;

public class InQueueStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        int id = (int) args[0];
        ICommand cmd = (ICommand) args[1];

        var queue = IoC.Resolve<Queue<ICommand>>("GetQueue", id);
        return new ActionCommand(() => { queue.Enqueue(cmd); });
    }
}
