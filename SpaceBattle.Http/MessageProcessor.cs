namespace SpaceBattle.Http;

public class MessageProcessor : IMessageProcessor {
    public HttpStatusCode ProcessMessage(Message message) {
        SpaceBattle.Lib.ICommand command = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Message.Processor", message);

        string threadID = IoC.Resolve<string>("Game.GetThreadIDByGameID", message.GameID);

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Senders.Send", threadID, command).Execute();

        return HttpStatusCode.OK;
    }
}
