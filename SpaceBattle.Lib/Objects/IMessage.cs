namespace SpaceBattle.Lib;

public interface IMessage
{
    public string CmdType { get; } 
    public int GameID { get; }
    public int ItemID { get; }
}
