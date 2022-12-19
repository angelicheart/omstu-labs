namespace SpaceBattle.Lib;

public interface IMoveStopable
{
    public IUObject obj { get; }
    public Vector velocity { get; }
    public IQueue<ICommand> queue { get; }
}
