using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FortunaPRNG
{
    public class Provider
    {
        private readonly Generator _generator = new Generator();
        private readonly Accumulator _accumulator = new Accumulator();

        private DateTime _lastReseed;

        public Provider()
        {
            Initialize();
        }

        private void Initialize()
        {
            while (!_accumulator.HasEnoughEntropy)
            {
                Thread.Sleep(1);
            }
            _generator.Reseed(_accumulator.ReadFromPool());
            _lastReseed = DateTime.UtcNow;
        }

        public int GetRandomInt()
        {
            if (_accumulator.HasEnoughEntropy && (DateTime.UtcNow - _lastReseed).TotalMilliseconds > 100)
            {
                _generator.Reseed(_accumulator.ReadFromPool());
                _lastReseed = DateTime.UtcNow;
            }
            return _generator.GenerateInt();
        }
    }
}