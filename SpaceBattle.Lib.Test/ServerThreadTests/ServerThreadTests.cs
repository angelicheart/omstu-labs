namespace SpaceBattle.Lib.Test;

public class UnitTests12
{
    [Fact]
    public void ServerThread_test1()
    {
        var queue1 = new BlockingCollection<ICommand>(100);
        var reciever = new Mock<IReciever>();

        // reciever.Setup(r => r.Recieve()).Returns(() =>  new ActionCommand(
        //     (arg) => {
        //         throw new System.Exception();
        //     }
        // ));

        reciever.Setup(r => r.Recieve()).Returns(() => queue1.Take());
        reciever.Setup(r => r.isEmpty()).Returns(true);

        var sender = new Mock<ISender>();

        sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

        ManualResetEvent mre = new ManualResetEvent(false);

        sender.Object.Send(new ActionCommand(
            (arg) => {
                Thread.Sleep(6000);
            } 
        ));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                //throw new System.Exception();
            } 
        ));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                mre.Set();
            } 
        ));
        Assert.Equal(3, queue1.Count);
        Assert.False(reciever.Object.isEmpty());
        
        ServerThread st = new ServerThread(reciever.Object);

        st.Execute();

        //Thread.Sleep(1000); //над больше чем в сендере

        mre.WaitOne();

        Assert.Equal(1, queue1.Count);
        Assert.True(reciever.Object.isEmpty());
    }

    [Fact]
    public void ServerThread_test12()
    {
        var queue1 = new BlockingCollection<ICommand>(100);
        var reciever = new Mock<IReciever>();

        reciever.Setup(r => r.Recieve()).Returns(() => queue1.Take());
        reciever.Setup(r => r.isEmpty()).Returns(() => queue1.Count == 0);

        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

        ServerThread st = new ServerThread(reciever.Object);

        sender.Object.Send(new ActionCommand(
            (arg) => {
                //throw new System.Exception(); Thread.Sleep(6000);
            } 
        ));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                //throw new System.Exception();
            } 
        ));

        var queue2 = new BlockingCollection<ICommand>(100);
        var reciever2 = new Mock<IReciever>();

        reciever.Setup(r => r.Recieve()).Returns(() => queue1.Take());
        reciever.Setup(r => r.isEmpty()).Returns(() => queue1.Count == 0);

        var sender2 = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

         ServerThread st2 = new ServerThread(reciever.Object);

        sender2.Object.Send(new ActionCommand(
            (arg) => {
                Thread.Sleep(100);
            } 
        ));

        sender2.Object.Send(new ActionCommand(
            (arg) => {
                //throw new System.Exception();
            } 
        ));

        st.Execute();
        st2.Execute();
    }
}

// manual reset event 
// barrier