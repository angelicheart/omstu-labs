namespace SpaceBattle.Lib;

public class InitializeScopeStrategy : IStrategy
{
    public object Execute(params object[] _args)
    {
        object scope = IoC.Resolve<object>("Scopes.Current");
        object scopenew = IoC.Resolve<object>("Game.GetScope", (string)_args[0]);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scopenew).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueDequeue", (Func<object[], ICommand>)(args => (ICommand)new FromQueueStrategy().Execute((Queue<ICommand>)args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueEnqueue", (Func<object[], ICommand>)(args => new InQueueCommand((Queue<ICommand>)args[0], (ICommand)args[1]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantum", (Func<object[], object>)(args => (int)_args[1])).Execute();

        Dictionary<string, IUObject> objects = new Dictionary<string, IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Objects", (Func<object[], Dictionary<string, IUObject>>)(args => objects)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.GetItem", (Func<object[], IUObject>)(args => (IUObject)new GetItemStrategy().Execute(args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.RemoveItem", (Func<object[], ICommand>)(args => new DeleteItemCommand((string)args[0]))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        return scopenew;
    }
}
