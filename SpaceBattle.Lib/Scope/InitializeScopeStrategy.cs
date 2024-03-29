namespace SpaceBattle.Lib;

public class InitializeScopeStrategy : IStrategy
{
    public object Execute(params object[] arg)
    {
        object currentScope = IoC.Resolve<object>("Scopes.Current");
        object newScope = IoC.Resolve<object>("Get.Scope", (string) arg[0]);
        Dictionary<string, IUObject> objects = new();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Dequeue", (object[] args) => (ICommand) new FromQueueStrategy().Execute((Queue<ICommand>) args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Enqueue", (object[] args) => new InQueueCommand((Queue<ICommand>) args[0], (ICommand) args[1])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Quantum", (object[] args) => arg[1]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Objects", (object[] args) => objects).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Item", (object[] args) => (IUObject)new GetItemStrategy().Execute(args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Item.Remove", (object[] args) => new DeleteItemCommand((string) args[0])).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();

        return newScope;
    }
}
