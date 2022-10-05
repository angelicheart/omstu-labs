namespace SpaceBattle.Lib.Objects
{
    public class Spaceship : IMovable
    {
        public Vector position { get; set; } = default;
        public Vector velocity { get; set; }
    }
}