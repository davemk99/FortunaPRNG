using FortunaPRNG;
using System;
using System.Collections;
using Xunit;

namespace TestPRNG
{
    public class ChiSquareTest
    {
        public bool IsRandom(int[] randomNums, int r)
        {
            int N = randomNums.Length;

            if (N <= 10 * r)
                return false;

            double N_r = N / r;
            double chi_square = 0;
            Hashtable HT;

            HT = this.RandomFrequency(randomNums);

            double f;
            foreach (DictionaryEntry Item in HT)
            {
                f = (int)(Item.Value) - N_r;
                chi_square += Math.Pow(f, 2);
            }
            chi_square = chi_square / N_r;

            var isInRange = Math.Abs(chi_square - r) <= 2 * Math.Sqrt(r);
            return isInRange;
        }

        private Hashtable RandomFrequency(int[] randomNums)
        {
            Hashtable HT = new Hashtable();
            int N = randomNums.Length;

            for (int i = 0; i <= N - 1; i++)
            {
                if (HT.ContainsKey(randomNums[i]))
                    HT[randomNums[i]] = (int)HT[randomNums[i]] + 1;
                else
                    HT[randomNums[i]] = 1;
            }

            return HT;
        }

        [Fact]
        public void ChiSquare()
        {
            var provider = new Provider();
            var arr = new int[200000];
            int maxNum = 1000;
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Math.Abs(provider.GetRandomInt() % maxNum) + 1;
            }
            Assert.True(this.IsRandom(arr, maxNum));
        }
    }
}