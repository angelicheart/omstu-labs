namespace SpaceBattle.Lib
{
    public class Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static Vector operator +(Vector v1, Vector v2)
        => new Vector(v1.X + v2.X, v1.Y + v2.Y);

        public override bool Equals(object obj)
        {
            var item = obj as Vector;

            if (item == null)
            {
                return false;
            }

            return item.X == X && item.Y == Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
