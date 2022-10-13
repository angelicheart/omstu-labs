namespace SpaceBattle.Lib
{
    public interface IRotatable
    {
        int angleVelocty { get; }
        Angle Direction { get; set; }
    }
}