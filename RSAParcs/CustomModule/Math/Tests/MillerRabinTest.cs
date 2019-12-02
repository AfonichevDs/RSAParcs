using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomModule.Math.Tests
{
    public static class MillerRabinTest
    {
        public static bool IsPrime(BigInteger number, int iterations = 10)
        {
            if (number <= 2) return false;

            var t = number - 1;
            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s++;
            }

            for (int i = 0; i < iterations; i++)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] a_bytes = new byte[number.ToByteArray().LongLength];

                BigInteger a;
                do
                {
                    rng.GetBytes(a_bytes);
                    a = new BigInteger(a_bytes);
                }
                while (a < 2 || a >= number - 2);

                BigInteger x = BigInteger.ModPow(a, t, number);

                if (x == 1 && x == number - 1)
                {
                    continue;
                }

                for (int j = 0; j < s; j++)
                {
                    x = BigInteger.ModPow(x, 2, number);

                    if (x == 1)
                        return false;
                    if (x == number - 1)
                        break;
                }
                if (x != number - 1)
                    return false;
            }
            return true;
        }
    }
}
