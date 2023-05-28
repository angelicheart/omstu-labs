namespace SpaceBattle.Http;

[DataContract(Name = "Message")]
public class Message {
    [DataMember(Name = "SimpleProperty", Order = 1)]
    public string GameID { get; set; }

    [DataMember(Name="CommandName")]
    public string CommandName { get; set; }

    [DataMember(Name="args")]
    public Dictionary<string, string> args { get; set; }
}
    