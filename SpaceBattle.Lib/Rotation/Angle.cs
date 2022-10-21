namespace SpaceBattle.Lib
{
    public class Angle
    {
        public int N { get; set; }
        public int D { get; set; }
        public Angle(int n, int d)
        {
            if ((double)n / d > 360)
            {   
                int gcd = GCD(n, d); 
                n /= gcd;
                d /= gcd;
                N = n % (360 * d);
                D = d;
            }
            else
            {
                int gcd = GCD(n, d);
                N = n / gcd;
                D = d / gcd;
            }
        }

        public Angle(int n)
        {
            N = n % 360;
            D = 1;
        }

        private int GCD(int n, int d)
        {
            int numerator = n;
            while (n != d)
            {
                if (n >= d)
                    n = n - d;
                else
                    d = d - n;
            }
            return n;
        }

        public static Angle operator +(Angle a1, Angle a2)
        {
            return new Angle(a1.N * a2.D + a2.N * a1.D,
            a1.D * a2.D);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(N, D);
        }

        public override bool Equals(object obj)
        {
            var item = obj as Angle;

            if (item == null)
            {
                return false;
            }
            return item.N == N && item.D == D;
        }
    }
}