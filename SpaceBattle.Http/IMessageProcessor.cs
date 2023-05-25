namespace SpaceBattle.Http;

[ServiceContract]
public interface IMessageProcessor
{
    [OperationContract]
    HttpStatusCode ProcessMessage(string messageJson);
}
