using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FortunaPRNG
{
    public class Accumulator
    {
        public bool HasEnoughEntropy => Pools[0].Size > 8;

        private int _runningCount;
        private int _requestDataCount;
        private Pool[] Pools = new Pool[32];

        public Accumulator()
        {
            for (int i = 0; i < Pools.Length; i++)
            {
                Pools[i] = new Pool();
            }
            Task.Run(new Action(Schedule));
        }

        private void Schedule()
        {
            var runningCount = _runningCount;
            _runningCount++;
            var poolSize = Pools.Length;
            var poolIndex = runningCount % poolSize;
            var data = BitConverter.GetBytes(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds());
            Pools[poolIndex].WriteToPool(data);
            Task.Delay(15).ContinueWith(x =>
            {
                Schedule();
            });
        }

        public byte[] ReadFromPool()
        {
            if (!HasEnoughEntropy)
            {
                throw new Exception("Not enough Entropy");
            }
            const int maxSeedSize = 32 * 32;
            var requestForDataCount = _requestDataCount++;
            var randomData = new byte[maxSeedSize];
            var bufferIndex = 0;
            for (var poolIndex = 0; poolIndex < 32; poolIndex++)
            {
                if (requestForDataCount % Math.Pow(2, poolIndex) != 0)
                {
                    // We can break out the first time we hit this condition
                    break;
                }

                var poolData = Pools[poolIndex].ReadFromPool();

                poolData.CopyTo(randomData, bufferIndex);
                bufferIndex += poolData.Length;
            }

            return randomData;
        }
    }
}