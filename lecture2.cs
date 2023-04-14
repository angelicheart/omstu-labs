using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class MyThread
{
    Thread thread;

    IReciver queue;
    bool stop = false;
    Action strategy;

    internal void Stop() => stop = true;

    internal void HandleCommand()
    {
        var cmd = queue.Recieve();

        cmd.Execute();
    }
    public MyThread(IReciver queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            HandleCommand();
        };

        thread = new Thread(() =>
        {
            while (!stop)
                strategy();
        });
    }

    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;

    }
    public void Execute()
    {
        thread.Start();
    }
}

class UpdateBehaviourCommand : ICommand
{
    Action behaviour;
    MyThread thread;

    public UpdateBehaviourCommand(MyThread thread, Action newBehaviour)
    {
        this.behaviour = newBehaviour;
        this.thread = thread;
    }
    public void Execute()
    {
        thread.UpdateBehaviour(newBehaviour);
    }
}

class ThreadStopCommand : ICommand
{
    MyThread stoppingThread;
    public ThreadStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;

    public void Execute()
    {
        if (Thread.CurrentThread == stoppingThread)
        {
            stoppingThread.Stop();
        }
        else
        {
            throw new Exception();
        }
    }
}

// class RecieverAdapter : IReciver
// {
//     BlockingCollection<ICommand> queue;

//     public RecieverAdapter(BlockingCollection<Icommand> queue) => this.queue = queue;

//     public ICommand Receive()
//     {
//         return queue.Take();
//     }

//     public bool isEmpty()
//     {
//         return queue.Count() == 0;
//     }
// }
class ExampleServer
{
    var queue = new BlockingCollection<ICommand>(1000);
    var reciever = new RecieverAdapter(queue);
    MyThread thread = new MyThread(new RecieverAdapter(queue));
    public void Execute()
    {
        thread.Execute();

        thread.Add(new ThreadStopCommand(thread));

        queue.Add(new UpdateBehaviourCommand(thread, () =>
        {
            if (queue.isEmpty())
            {
                thread.Stop();
            }
            else
            {
                thread.HandleCommand();
            }
        }
        ));
    }
}

class GameCommand : ICommand
{
    IQueue<ICommand> queue = new Queue<ICommand>(40);

    object scope;

    public GameCommand(object scope)
    {
        this.scope = scope;

        IoC.REsolve<ICommand>("Scope.Current.Set", scope).Execute();

        Dictionary<string, UObject> gameItems = new Dictionary<string, UObject>();
        IoC.Resolve<InterpretCommand>("IoC.Register", "GameItems.Get", (args) => gameItems((string)args[0])).Execute();
        var queue = new Queue();
        var sender = new SenderWarper(queue);

        IoC.Resolve<InterpretCommand>("IoC.Register", "Queue.Send", (args) => sender).Execute();
        IoC.Resolve<InterpretCommand>("IoC.Register", "Queue.Reciever", (args) => reciver).Execute();

        orderToCommand["Move"] = "Movement";
        orderToCommand["roatete"]

        Dictionary<string, string> orderToCommand = new Dictionary<string, string>();
        IoC.Resolve<InterpretCommand>("IoC.Register", "Commands.Create", (args) => {
            var obj = (UObject) args[0];
            return IoC.Resolve<InterpretCommand>(orderToCommand[((IMessage)args[1])], obj)
        }).Execute();



    }
    public void Execute()
    {
        IOC.Resolve<ICommand>("Scope.Current.Set", scope).Execute();

        IReciver reciver = IoC.Resolve<IReciver>("Queue.Reciever");

        while(если время выполнения игры меньше выделенного кванта)
        {
            //время начала операции 
            reciver.Recieve.Execute();
            //время завершения операции
            //увеличить время выполнения игры
        }
    }
}

interface ISender
{
    void Send(object message);
}

ConcurrentDictionary<string, ISender> senders = new ConcurrentDictionary<string, ISender>();

IOC.Resolve<ICommand>("Sender", obj).Execute();

class SendCommand : ICommand
{
    IMessage message;

    public SendCommand(IMessage message) => this.message = message;

    public void Execute()
    {
        ISender sender;
        IOC.Resolve<ConcurrentDictionary<string, ISender>("SendersMap").TryGetValue(message.Hash, out sender)
        {
            sender.Send(message);
        }
        else
        {
            Exception;
        }
    }
}

class InterpretCommand : InterperCommand
{
    InterpretingMessage message;

    public InterpretCommand(InterpetingMessage message) => this.message = message;

    public void Execute()
    {
        // id корабля
        // приказ
        // specific params

        var obj = IoC.Resolve<UObject>("GameItems", message.Id);

        ICommand cmd;
        switch(message.order)
        {
            case 1: cmd = IoC.Resolve<ICommand>("Command.Movement", obj, message.InitialVelocity);
        }
        ICommand cmd = IoC.Resolve<ICommand>("Commands.Create", obj, message);

        IoC.Resolve<ISender>("Queue", cmd);

    }
}

IoC.Resolve<ICommand>("IoC.register", "GameItems.Get", (args) =>

);


soft stop

while !stop {
    if !queue.isEmpty() {
        strategy();
    }
    else
    Action.exectue,
    thread stop
}