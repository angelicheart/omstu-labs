namespace SpaceBattle.Lib.Objects
{
    public class Spaceship : IObj
    {
        public Vector position { get; set; } = default;
        public Vector velocity { get; set; }
    }
}