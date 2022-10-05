namespace SpaceBattle.Lib
{
    public class Angle
    {
        public double angle { get; set; }

        public Angle(double Direction)
        {
            angle = Direction;
        }

        public Angle Rotate(double avelocity)
        {
            return new Angle((angle + avelocity) % (2 * Math.PI));
        }

        public override bool Equals(object obj)
        {
            var item = obj as Angle;

            if (item == null)
            {
                return false;
            }

            return item.angle == angle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(angle);
        }
    }
}