using CustomModule.Math.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomModule.Math.Providers
{
    public class PrimeBigIntProvider : IProvidePrime<BigInteger>
    {
        static object locker = new object();

        public BigInteger Get(int n)
        {
            if (n % 8 != 0) return 0;
            BigInteger res = 0;
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 20; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    BigInteger number;
                    byte[] bytes = new byte[n / 8];
                    var rng = new RNGCryptoServiceProvider();
                    do
                    {
                        lock (locker)
                        {
                            if (!res.IsZero) return;
                        }
                        rng.GetBytes(bytes);
                        number = new BigInteger(bytes);
                        if (number < 0)
                        {
                            number *= -1;
                        }
                    }
                    while (!MillerRabinTest.IsPrime(number, 10));
                    lock (locker)
                    {
                        res = number;
                    }
                }));

            }

            Task.WaitAll(tasks.ToArray());
            return res;
        }
    }
}
