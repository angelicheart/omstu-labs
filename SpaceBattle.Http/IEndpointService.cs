namespace SpaceBattle.Http;

[ServiceContract]
public interface IEndpointService
{
    [OperationContract]
    void ProcessMessage(Stream messageStream);
}
