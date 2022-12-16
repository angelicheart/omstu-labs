namespace SpaceBattle.Lib;

public class QueuePushStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var command = new QueuePush((IQueue<ICommand>)args[0], (SpaceBattle.Lib.ICommand)args[1]);
        return command;
    }
}

public class QueuePush : ICommand
{
    private IQueue<ICommand> obj;
    private ICommand value;
    public QueuePush(IQueue<ICommand> obj, ICommand value)
    {   
        this.obj = obj;
        this.value = value;
    }
    public void Execute()
    {
        obj.Push(value);
    }
}
