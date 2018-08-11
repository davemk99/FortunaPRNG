using System.Linq;
using System.Security.Cryptography;

namespace FortunaPRNG
{
    internal class Pool
    {
        private readonly SHA256 _sha256 = SHA256.Create();

        private int _runningSize;
        public int Size { get => _runningSize; }

        private byte[] _hash = new byte[0];

        public void WriteToPool(byte[] data)
        {
            var bytes = new[] { (byte)data.Length }.Concat(data).ToArray();
            var shaBytes = _sha256.ComputeHash(bytes);
            _hash = shaBytes;
            _runningSize = _hash.Length;
        }

        public byte[] ReadFromPool()
        {
            _runningSize = 0;
            return _hash;
        }
    }
}