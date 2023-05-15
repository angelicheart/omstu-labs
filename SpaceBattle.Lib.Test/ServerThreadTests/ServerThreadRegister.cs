namespace SpaceBattle.Lib.Test;

public class ServerThreadRegistryClass {
    public static void ServerThreadRegistry() {
        var tds = new ThreadsDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain", (object[] args) => tds.Execute(args)).Execute();

        var rds = new ReceiversDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Receivers.Domain", (object[] args) => rds.Execute(args)).Execute();

        var sds = new SendersDomainStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Domain", (object[] args) => sds.Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain.Set", (object[] args) => new ThreadsDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.Domain.Get", (object[] args) => new ThreadsDomainGetStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Receivers.Domain.Set", (object[] args) => new ReceiversDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Receivers.Domain.Get", (object[] args) => new ReceiversDomainGetStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Domain.Set", (object[] args) => new SendersDomainSetStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Domain.Get", (object[] args) => new SendersDomainGetStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Senders.Send", (object[] args) => new SendCommandStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.CreateAndStart", (object[] args) => new CreateAndStartThreadStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.HardStop", (object[] args) => new HardStopThreadStrategy().Execute(args)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Threads.SoftStop", (object[] args) => new SoftStopThreadStrategy().Execute(args)).Execute(); 

        var Strategy = new Mock<SpaceBattle.Lib.IStrategy>();
        Strategy.Setup(c => c.Execute());

        var StrategyStrategy = new Mock<IStrategy>();
        StrategyStrategy.Setup(s => s.Execute(It.IsAny<object[]>())).Returns(Strategy.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.FindExceptionHandlerForCmd", (object[] args) => StrategyStrategy.Object.Execute(args)).Execute();
    }
}
