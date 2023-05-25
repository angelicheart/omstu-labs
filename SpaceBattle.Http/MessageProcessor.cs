namespace SpaceBattle.Http;

public class MessageProcessor : IMessageProcessor
{
    public HttpStatusCode ProcessMessage(string jsonMessage) {
        var message = JsonConvert.DeserializeObject<Message>(jsonMessage);

        string GameID = message.GameID;
        string CommandName = message.CommandName;

        string[] args;
        if (message.args != null) {
            args = message.args.Values.ToArray();
        }
        else {
            args = null;
        }
        
        SpaceBattle.Lib.ICommand command = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Command.Generate", CommandName, args);

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Senders.Send", GameID, command).Execute();

        return HttpStatusCode.OK;
    }
}
