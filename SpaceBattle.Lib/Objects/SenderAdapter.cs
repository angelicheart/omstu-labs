namespace SpaceBattle.Lib;

public class SenderAdapter : ISender
{
    BlockingCollection<ICommand> queue;

    public SenderAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public void Send(ICommand command)
    {
        queue.Add(command);
    }
}
