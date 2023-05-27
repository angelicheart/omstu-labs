namespace SpaceBattle.Http;

[DataContract]
public class Message {
    [DataMember(Name ="GameID")]
    public string GameID { get; set; }

    [DataMember(Name ="CommandName")]
    public string CommandName { get; set; }

    [DataMember(Name ="args")]
    public Dictionary<string, string> args { get; set; }
}
    