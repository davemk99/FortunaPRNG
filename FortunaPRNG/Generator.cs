using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FortunaPRNG
{
    public class Generator
    {
        private GeneratorState _state;

        public Generator()
        {
            var bytes = BitConverter.GetBytes(Environment.TickCount);
            InitializeGenerator(bytes);
        }

        private void InitializeGenerator(byte[] bytes)
        {
            _state = new GeneratorState
            {
                Counter = new System.Numerics.BigInteger(),
                Key = new byte[32]
            };
            Reseed(bytes);
        }

        public void Reseed(byte[] seed)
        {
            using (var sha = SHA256.Create())
            {
                var newKey = GetNewKey(sha.ComputeHash(_state.Key.Concat(seed).ToArray()));
                var counter = ++_state.Counter;
                _state = new GeneratorState
                {
                    Counter = counter,
                    Key = newKey
                };
            }
        }

        public int GenerateInt()
        {
            var size = 4;
            using (Aes aes = Aes.Create())
            {
                aes.Key = _state.Key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                var counterBytes = _state.CounterBytes;
                aes.IV = counterBytes;
                using (var cryptor = aes.CreateEncryptor())
                {
                    var cryptedBytes = cryptor.TransformFinalBlock(counterBytes, 0, 16);
                    var numberBytes = new byte[size];
                    Array.Copy(cryptedBytes, numberBytes, 4);
                    var randomNumber = BitConverter.ToInt32(numberBytes, 0);
                    _state.Counter++;
                    return randomNumber;
                }
            }
        }

        private byte[] GetNewKey(byte[] bytes)
        {
            Array.Resize(ref bytes, 32);

            return bytes;
        }
    }
}