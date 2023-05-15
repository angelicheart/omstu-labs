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
        String MovableAdapterCode = @"class MovableAdapter : IMovable {
        Object target;
        public MovableAdapter(Object target) => this.target = target; 
            Vector position {
            get { return IoC.Resolve<Vector>(""Game.position.Get"", target); }
            set { IoC.Resolve<ICommand>(""Game.position.Set"", target, value).Execute(); }
            }
            Vector velocity {
            get { return IoC.Resolve<Vector>(""Game.velocity.Get"", target); }
            }
        }";

        String RotatableAdapterCode = @"class RotatableAdapter : IRotatable {
        Object target;
        public RotatableAdapter(Object target) => this.target = target; 
            Angle angleVelocty {
            get { return IoC.Resolve<Angle>(""Game.angleVelocty.Get"", target); }
            }
            Angle Direction {
            get { return IoC.Resolve<Angle>(""Game.Direction.Get"", target); }
            set { IoC.Resolve<ICommand>(""Game.Direction.Set"", target, value).Execute(); }
            }
        }";

        String SenderAdapterCode = @"class SenderAdapter : ISender {
        Object target;
        public SenderAdapter(Object target) => this.target = target; 
        Void Send(SpaceBattle.Lib.ICommand);
        }";

        // MethodInfo[] methods = typeof(ISender).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        // foreach (MethodInfo m in methods) {
        //     output.WriteLine("" + m);
        // } 

        output.WriteLine(IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(ISender), typeof(object)));


        // Assert.Equal(MovableAdapterCode, IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(IMovable), typeof(object)));
        // Assert.Equal(RotatableAdapterCode, IoC.Resolve<String>("Game.Reflection.GenerateAdapterCode", typeof(IRotatable), typeof(object)));
    }
}
