namespace SpaceBattle.Lib
{
    public interface IUobject
    {
        object getProperty(string key);
        void setProperty(string key, object value);
    }

    public interface UObjectDelitableProperty
    {
    }

    public interface IStrategy
    {
        object Execute(params object[] args); 
    }

    public class IoC
    {
        static IDictionary<string, IStrategy> store;

        static IoC()
        {
            store = new Dictionary<string, IStrategy>();
            store["IoC.Add"] = new IoCAddStrategy(store);
            register`
            геттер стора через стратегию
        }

        public static T resolve<T>(string key, params object[] args)
        {
            return (T) store[key].Execute(args);
            // return (T) store["IoC.Resolve"].Execute(key, args);
        }   
    }

    class IoCAddCommand : ICommand
    {
        IDictionary<string, IStrategy> store;
        string key;
        IStrategy strategy;

        public IoCAddCommand(IDictionary<string, IStrategy> store, string key, IStrategy strategy)
        {
            this.store = store;
            this.key = key;
            this.strategy = strategy;
        }

        public void Execute()
        {
            store[key] = strategy;
        }
    }

    public class IoCAddStrategy: IStrategy
    {
        private IDictionary<string, IStrategy> store;

        public IoCAddStrategy (IDictionary<string, IStrategy> store)
        {
            this.store = store;
        }

        public object Execute(params object[] args)
        {
            string key = (string) args[0];
            IStrategy strategy = (IStrategy) args[1];
            return new IoCAddCommand(this.store, key, strategy);
        }
    }

    public class IoCResolve : IStrategy
    {

    }

    public class EmptyCommandStrategy : IStrategy
    {
        ICommand emptycommand;
        public EmptyCommandStrategy()
        {
            emptycommand = new EmptyCommand();
        }

        public object Execute(params object[] args)
        {
            return emptycommand;
        }
    }

    public class EmptyCommand : ICommand
    {
        public void Execute()
        {

        }
    }
}