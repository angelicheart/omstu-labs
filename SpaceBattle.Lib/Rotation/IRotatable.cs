namespace SpaceBattle.Lib
{
    public interface IRotatable
    {
        Angle angleVelocty { get; }
        Angle Direction { get; set; }
    }
}