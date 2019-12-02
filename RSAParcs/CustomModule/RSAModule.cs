using CustomModule.RSA;
using Parcs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomModule
{
    public class RSAModule : IModule
    {
        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            BigInteger publicK = BigInteger.Parse(info.Parent.ReadString());
            BigInteger shared = BigInteger.Parse(info.Parent.ReadString());
            string text = info.Parent.ReadString();

            RSAKey publicKey = new RSAKey(publicK, shared);

            var result = RSAProvider.Encrypt2(text, publicKey);
            var trueResult = result.Aggregate((x, y) => x + y);
            info.Parent.WriteData(trueResult.ToString());
        }
    }
}
