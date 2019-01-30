using BenchmarkDotNet.Attributes;
using System;

namespace PerfTests
{
    public class Test04
    {
        private const int Length = 10000;
        private int[,] _values;

        [GlobalSetup]
        public void Setup()
        {
            _values = new int[Length, Length];
            var rnd = new Random();
            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    _values[i, j] = rnd.Next();
                }
            }
        }

        [Benchmark(Baseline = true)]
        public void RowAccess()
        {
            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    var value = _values[i, j];
                }
            }
        }

        [Benchmark]
        public void ColumnAccess()
        {
            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    var value = _values[j, i];
                }
            }
        }

    }
}
