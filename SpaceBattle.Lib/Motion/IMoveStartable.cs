namespace SpaceBattle.Lib;

public interface IMoveStartable
{
    public IUObject obj { get; }
    public Vector velocity { get; }
}
