using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CustomModule.Math
{
    public static class MathHelper
    {
        public static BigInteger ExtendedEuclidian(BigInteger a, BigInteger b)
        {
            if (a > b)
            {
                BigInteger temp = a;
                a = b;
                b = temp;
            }

            BigInteger r1 = a, r2 = b, gcd = 0, a_copy = a, b_copy = b;
            ex_al_eu(r1, r2, ref gcd, ref a_copy, ref b_copy);
            return a_copy < 0 ? a_copy + b : a_copy;
        }

        private static void ex_al_eu_in(BigInteger r1, BigInteger r2, BigInteger x1, BigInteger x2, BigInteger y1,
            BigInteger y2, ref BigInteger gcd, ref BigInteger a, ref BigInteger b)
        {
            BigInteger r3 = r1 - r2 * (r1 / r2);
            BigInteger x3 = x1 - x2 * (r1 / r2);
            BigInteger y3 = y1 - y2 * (r1 / r2);
            if (r3 > 0)
                ex_al_eu_in(r2, r3, x2, x3, y2, y3, ref gcd, ref a, ref b);
            else
            {
                gcd = r2;
                a = x2;
                b = y2;
            }
        }

        private static void ex_al_eu(BigInteger r1, BigInteger r2, ref BigInteger gcd, ref BigInteger a, ref BigInteger b)
        {
            ref BigInteger a_t = ref (r1 > r2 ? ref a : ref b);
            ref BigInteger b_t = ref (r1 < r2 ? ref a : ref b);

            ex_al_eu_in(r1 > r2 ? r1 : r2, r1 < r2 ? r1 : r2, 1, 0, 0, 1, ref gcd, ref a_t, ref b_t);
        }
    }
}
