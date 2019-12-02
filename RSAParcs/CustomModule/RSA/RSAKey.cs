using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CustomModule.RSA
{
    public struct RSAKey
    {
        public RSAKey(BigInteger keyOne, BigInteger keyTwo)
        {
            KeyOne = keyOne;
            KeyTwo = keyTwo;
        }

        public BigInteger KeyOne { get; set; }
        public BigInteger KeyTwo { get; set; }
    }
}
