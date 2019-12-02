using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomModule.Math.Providers
{
    public interface IProvidePrime<T>
    {
        T Get(int n);
    }
}
