namespace SpaceBattle.Lib.Test;

public class DomainsTests
{
    public DomainsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var rds = new RecieversDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Recievers.Domain", (object[] args) => rds.Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Recievers.Domain.Set", (object[] args) => new RecieversDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Recievers.Domain.Get", (object[] args) => new RecieversDomainGetStrategy().Execute(args)).Execute();
    }
    
    [Fact]
    public void DomainSetGetTest_succ()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();

        ICommand command = new EmptyCommand();

        queue.Add(command);

        var reciever = new Mock<RecieverAdapter>(queue);

        IoC.Resolve<ICommand>("Game.Recievers.Domain.Set", "1", reciever.Object).Execute();

        Assert.Equal(command, IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "1").Recieve());
    }

    [Fact]
    public void DomainSetGetTest_unsucc()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();

        queue.Add(new ExceptionCommand());

        var reciever = new Mock<RecieverAdapter>(queue);

        IoC.Resolve<ICommand>("Game.Recievers.Domain.Set", "1", reciever.Object).Execute();

        Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => IoC.Resolve<RecieverAdapter>("Game.Recievers.Domain.Get", "2").Recieve());
    }
}
