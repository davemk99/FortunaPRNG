using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FortunaPRNG
{
    internal struct GeneratorState
    {
        internal byte[] Key;
        internal BigInteger Counter;

        internal byte[] CounterBytes
        {
            get
            {
                var bytes = new byte[16];
                var counterBytes = Counter.ToByteArray();

                Array.Copy(counterBytes, bytes, counterBytes.Length);

                return bytes;
            }
        }
    }
}