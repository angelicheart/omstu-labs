namespace SpaceBattle.Http;

public class MessageProcessor : IMessageProcessor {
    public HttpStatusCode ProcessMessage(Message message) {
        SpaceBattle.Lib.ICommand command = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Message.Processor", message);

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Senders.Send", message.GameID, command).Execute();

        return HttpStatusCode.OK;
    }
}
