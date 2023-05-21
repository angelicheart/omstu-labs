namespace SpaceBattle.Lib.Test;
using Xunit.Abstractions;

public class AdapterCodeGeneratorTests
{
    private readonly ITestOutputHelper output;
    public AdapterCodeGeneratorTests(ITestOutputHelper output)
    {
        this.output = output;
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Reflection.GenerateAdapterCode", (object[] args) => new AdapterCodeGeneratorStrategy().Execute(args)).Execute(); 
    }

    [Fact]
    public void AdapterCodeGeneratorTest_1()
    {
        // public interface IReceiver
        // {
        //     ICommand Receive();
        //     bool isEmpty();
        // }
        
        String ReciverAdapterGenericTargetCode = 
        @"class ReceiverAdapter : IReceiver {
        BlockingCollection<ICommand> target;
        public ReceiverAdapter(BlockingCollection<ICommand> target) => this.target = target; 
        public ICommand Receive () {
            return IoC.Resolve<ICommand>(""Game.Receive.Strategy"", target);
        }
        public Boolean isEmpty () {
            return IoC.Resolve<Boolean>(""Game.isEmpty.Strategy"", target);
        }
    }";

        // public interface IMoveStartable
        // {
        //     public IUObject obj { get; }
        //     public Vector velocity { get; }
        //     public IQueue<ICommand> queue { get; }
        // }

        String MoveStartableAdapterCode = 
        @"class MoveStartableAdapter : IMoveStartable {
        Object target;
        public MoveStartableAdapter(Object target) => this.target = target; 
        public IUObject obj {
               get { return IoC.Resolve<IUObject>(""Game.obj.Get"", target); }
        }
        public Vector velocity {
               get { return IoC.Resolve<Vector>(""Game.velocity.Get"", target); }
        }
        public IQueue<ICommand> queue {
               get { return IoC.Resolve<IQueue<ICommand>>(""Game.queue.Get"", target); }
        }
    }";

        // public interface ISender {
        //     void Send(ICommand command);
        // }   

        String SenderAdapterCode = 
        @"class SenderAdapter : ISender {
        BlockingCollection<ICommand> target;
        public SenderAdapter(BlockingCollection<ICommand> target) => this.target = target; 
        public void Send (ICommand command) {
            IoC.Resolve<ICommand>(""Game.Send.Command"", target, command).Execute();
        }
    }";

        // public interface IMovable
        // {
        //     Vector position { get; set; }
        //     Vector velocity { get; }
        // }

        String MovableAdapterCode = 
        @"class MovableAdapter : IMovable {
        Vector target;
        public MovableAdapter(Vector target) => this.target = target; 
        public Vector position {
               get { return IoC.Resolve<Vector>(""Game.position.Get"", target); }
               set { IoC.Resolve<ICommand>(""Game.position.Set"", target, value).Execute(); }
        }
        public Vector velocity {
               get { return IoC.Resolve<Vector>(""Game.velocity.Get"", target); }
        }
    }";

        Assert.Equal(ReciverAdapterGenericTargetCode, IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(IReceiver), typeof(BlockingCollection<ICommand>)));
        Assert.Equal(MoveStartableAdapterCode, IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(IMoveStartable), typeof(object)));
        Assert.Equal(SenderAdapterCode, IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(ISender), typeof(BlockingCollection<ICommand>)));
        Assert.Equal(MovableAdapterCode,IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(IMovable), typeof(Vector)));
    }
}
