namespace SpaceBattle.Lib.Test;

public class DomainsTests
{
    public DomainsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var rds = new ReceiversDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Receivers.Domain", (object[] args) => rds.Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Receivers.Domain.Set", (object[] args) => new ReceiversDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Receivers.Domain.Get", (object[] args) => new ReceiversDomainGetStrategy().Execute(args)).Execute();
    }
    
    [Fact]
    public void DomainSetGetTest_succ()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();

        ICommand command = new EmptyCommand();

        queue.Add(command);

        var Receiver = new Mock<ReceiverAdapter>(queue);

        IoC.Resolve<ICommand>("Game.Receivers.Domain.Set", "1", Receiver.Object).Execute();

        Assert.Equal(command, IoC.Resolve<IReceiver>("Game.Receivers.Domain.Get", "1").Receive());
    }

    [Fact]
    public void DomainSetGetTest_unsucc()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();

        queue.Add(new EmptyCommand());

        var Receiver = new Mock<ReceiverAdapter>(queue);

        IoC.Resolve<ICommand>("Game.Receivers.Domain.Set", "1", Receiver.Object).Execute();

        Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => IoC.Resolve<IReceiver>("Game.Receivers.Domain.Get", "2").Receive());
    }
}
