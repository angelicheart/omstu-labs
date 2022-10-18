namespace SpaceBattle.Lib
{
    public class Angle
    {   
        public int N { get; set; }
        public int D { get; set; }
        public Angle(int n, int d)
        {   
            if((double) n / d > 360)
            {   
                n /= GCD(n, d);
                d /= GCD(n, d);
                N = n % (360 * d);
                D = d;
            }
            else 
            {
                N = n / GCD(n, d);
                D = d / GCD(n, d);
            }
        }

        public Angle(int n)
        {
            N = n % 360;
            D = 1;
        }
        
        public int GCD(int n, int d)
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

        // void Seek(ref int a, ref int b, int ainc, int binc, Func<int, int, bool> f)
        // {       
        //     a += ainc;
        //     b += binc;

        //     if (f(a, b))
        //     {
        //         int weight = 1;

        //         do
        //         {
        //             weight *= 2;
        //             a += ainc * weight;
        //             b += binc * weight;
        //         }
        //         while (f(a, b));

        //         do
        //         {
        //             weight /= 2;

        //             int adec = ainc * weight;
        //             int bdec = binc * weight;

        //             if (!f(a - adec, b - bdec))
        //             {
        //                 a -= adec;
        //                 b -= bdec;
        //             }
        //         }       
        //     while (weight > 1);
        //     }
        // }

        // public (int, int) RealToFraction(double value)
        // {
        //     double accuracy = 1.0E-9;

        //     int sign = Math.Sign(value);

        //     if (sign == -1)
        //     {
        //         value = Math.Abs(value);
        //     }

        //     double maxError = sign == 0 ? accuracy : value * accuracy;

        //     int n = (int) Math.Floor(value);
        //     value -= n;

        //     if (value < maxError)
        //     {
        //         return (sign * n, 1);
        //     }

        //     if (1 - maxError < value)
        //     {
        //         return (sign * (n + 1), 1);
        //     }

        //     int lower_n = 0;
        //     int lower_d = 1;

        //     int upper_n = 1;
        //     int upper_d = 1;

        //     while (true)
        //     {
        //         int middle_n = lower_n + upper_n;
        //         int middle_d = lower_d + upper_d;

        //         if (middle_d * (value + maxError) < middle_n)
        //         {
        //             Seek(ref upper_n, ref upper_d, lower_n, lower_d, (un, ud) => (lower_d + ud) * (value + maxError) < (lower_n + un));
        //         }
        //         else if (middle_n < (value - maxError) * middle_d)
        //         {
        //             Seek(ref lower_n, ref lower_d, upper_n, upper_d, (ln, ld) => (ln + upper_n) < (value - maxError) * (ld + upper_d));
        //         }
        //         else
        //         {
        //             return ((n * middle_d + middle_n) * sign, middle_d);
        //         }
        //     }
        // }
    }
}