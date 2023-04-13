namespace SpaceBattle.Lib;

public class RecieverAdapter : IReciever
{
    BlockingCollection<ICommand> queue;

    public RecieverAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Recieve()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}