namespace SpaceBattle.Lib;

public class GetQueueStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        // получение очереди из игры по айди
        int id = (int) args[0];

        Queue<ICommand> queue;

        IoC.Resolve<"получение очереди из игры по айдишник">;

        return queue;
    }
}