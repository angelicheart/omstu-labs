namespace SpaceBattle.Lib.Test;

public class UpdateBehaviourTests
{
    public UpdateBehaviourTests()
    {
        ServerThreadRegistryClass.ServerThreadRegistry();
    }

    [Fact(Timeout = 100)]
    public void UpdateBehaviour_succ()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>(100);

        queue.Add(new EmptyCommand());
        queue.Add(new EmptyCommand());
        queue.Add(new EmptyCommand());

        RecieverAdapter reciever = new RecieverAdapter(queue);

        SenderAdapter sender = new SenderAdapter(queue);

        ServerThread st = new ServerThread(reciever);

        st.UpdateBehaviour(() => {st.Stop(); Assert.True(st.stop);});

        st.Start();
    }
}