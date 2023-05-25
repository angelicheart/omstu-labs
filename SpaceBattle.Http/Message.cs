namespace SpaceBattle.Http;

[DataContract]
public class Message
{
    [DataMember(Name = "game id")]
    public string GameID { get; set; }

    [DataMember(Name = "CommandName")]
    public string CommandName { get; set; }

    [DataMember(Name = "args")]
    public int args { get; set; }
}
