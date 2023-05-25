namespace SpaceBattle.Http;

public class Message {
    [JsonProperty("GameID")]
    public string GameID { get; set; }

    [JsonProperty("CommandName")]
    public string CommandName { get; set; }

    [JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, string> args { get; set; }
}
    