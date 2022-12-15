namespace SpaceBattle.Lib;

public interface IMoveStartable
{
    public IUObject obj { get; }
    public Vector velocity { get; }
    public IQueue<ICommand> queue { get; }
}
