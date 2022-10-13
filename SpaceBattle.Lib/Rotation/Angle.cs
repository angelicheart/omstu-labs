namespace SpaceBattle.Lib
{
    public class Angle
    {
        public int angle { get; set; }

        public Angle(int Direction)
        {
            angle = Direction;
        }

        public Angle Rotate(int avelocity)
        {
            return new Angle((angle + avelocity) % 360);
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