namespace SpaceBattle.Lib.Test;

public class UnitTests12
{
    [Fact]
    public void ServerThread_test()
    {
        var queue1 = new BlockingCollection<ICommand>();
        var reciever = new RecieverAdapter(queue1);

        var sender = new Mock<ISender>();

        sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                new EmptyCommand();
            } 
        ));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                new EmptyCommand();
            } 
        ));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                throw new Exception();
            } 
        ));

        Assert.Equal(3, queue1.Count);
        Assert.False(reciever.isEmpty());

        ServerThread st = new ServerThread(reciever);

        st.Execute();

        Assert.Equal(0, queue1.Count);
        Assert.True(reciever.isEmpty());
    }

        [Fact]
    public void ServerThread_test1()
    {
        var queue1 = new BlockingCollection<ICommand>(100);

        var reciever = new RecieverAdapter(queue1);

        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

        ServerThread st = new ServerThread(reciever);

        sender.Object.Send(new ActionCommand(
            (arg) => {
                throw new System.Exception();
            } 
        ));

        sender.Object.Send(new ActionCommand(
            (arg) => {
                throw new System.Exception();
            } 
        ));

        var queue2 = new BlockingCollection<ICommand>(100);

        var reciever2 = new RecieverAdapter(queue1);

        var sender2 = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<ICommand>())).Callback<ICommand>((command) => queue1.Add(command));

        ServerThread st2 = new ServerThread(reciever);

        sender2.Object.Send(new ActionCommand(
            (arg) => {
                throw new System.Exception();
            } 
        ));

        sender2.Object.Send(new ActionCommand(
            (arg) => {
                throw new System.Exception();
            } 
        ));

        st.Execute();
        st2.Execute();
    }
}
