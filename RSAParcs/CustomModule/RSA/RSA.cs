using CustomModule.Constants;
using CustomModule.Math;
using CustomModule.Math.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CustomModule.RSA
{
    public class RSAProvider
    {
        private KeySize keySize;
        private readonly BigInteger number1;
        private readonly BigInteger number2;
        private readonly BigInteger composition;
        private readonly BigInteger eulerFunc;
        private readonly BigInteger publicExhibitor;
        private readonly BigInteger privateExhibitor;

        public RSAKey PublicKey { get; }
        public RSAKey PrivateKey { get; }
        public BigInteger SessionKey { get; }

        public RSAProvider(KeySize size)
        {
            keySize = size;
            number1 = InitializeRandom();
            number2 = InitializeRandom();

            composition = number1 * number2;
            eulerFunc = (number1 - 1) * (number2 - 1);

            publicExhibitor = GetPublicExhibitor();
            privateExhibitor = GetPrivateExhibitor();
            PublicKey = new RSAKey(publicExhibitor, composition);
            PrivateKey = new RSAKey(privateExhibitor, composition);
            //SessionKey = InitializeRandom();
        }

        public RSAProvider() : this(KeySize.Bit512) { }

        private BigInteger InitializeRandom()
        {
            PrimeBigIntProvider rand = new PrimeBigIntProvider();
            switch (keySize)
            {
                case KeySize.Bit64:
                    return rand.Get(64);
                case KeySize.Bit512:
                    return rand.Get(512);
                case KeySize.Bit1024:
                    return rand.Get(1024);
                default:
                    return rand.Get(512);
            }
        }

        //private BigInteger GetSessionKey()
        //{ 
        //    var m = InitializeRandom();
        //    return BigInteger.ModPow()
        //}

        private BigInteger GetPublicExhibitor() =>
            FermatNumbers.Numbers.ElementAt(new Random().Next(FermatNumbers.Numbers.Count));

        public BigInteger GetPrivateExhibitor() =>
            MathHelper.ExtendedEuclidian(publicExhibitor, eulerFunc);

        public BigInteger DecryptInt(BigInteger message)
        {
            return BigInteger.ModPow(message, publicExhibitor, composition);
        }

        public static string Decrypt(BigInteger message, RSAKey key)
        {
            var bi = BigInteger.ModPow(message, key.KeyOne, key.KeyTwo);

            string output = Encoding.UTF8.GetString(bi.ToByteArray());
            return output;
        }

        private static BigInteger Encrypt(BigInteger message, RSAKey key)
        {
            return BigInteger.ModPow(message, key.KeyOne, key.KeyTwo);
        }

        public static BigInteger Encrypt(string message, RSAKey key)
        {
            BigInteger bi = new BigInteger(Encoding.UTF8.GetBytes(message));
            return Encrypt(bi, key);
        }

        public static List<string> Encrypt2(string message, RSAKey key)
        {
            List<string> res = new List<string>();
            var keyLength = key.KeyTwo.ToByteArray().Length;
            var limit = keyLength / 8;
            if (message.Length > limit)
            {
                var iterations = System.Math.Ceiling((float)message.Length / (float)(limit));
                for (int i = 0; i < iterations; i++)
                {
                    var t = new string(message.Skip(i * (limit)).Take((limit)).ToArray());
                    res.Add(Encrypt(t, key).ToString());
                }
            }
            else res.Add(Encrypt(message, key).ToString());
            return res;
        }

        public static string Decrypt(List<BigInteger> message, RSAKey key)
        {
            string decrypted = "";

            foreach (var item in message)
            {
                decrypted += Decrypt(item, key);
            }
            return decrypted;
        }
    }
}
