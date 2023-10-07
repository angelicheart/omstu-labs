namespace SpaceBattle.Lib;

public class GetQueueStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        // получение очереди из игры по айди
        int id = (int) args[0];

        Queue<ICommand> queue;

        IoC.Resolve<IDictionary<int, Queue<ICommand>>>("GetGameQueue").TryGetValue(id, out queue);

        return queue;
    }
}
