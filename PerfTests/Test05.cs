using BenchmarkDotNet.Attributes;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace PerfTests
{
    public class Test05
    {
        private const int Length = 10000;
        private byte[] _byteArray;
        private byte[] _byteArray2;

        [GlobalSetup]
        public void Setup()
        {
            _byteArray = new byte[Length];
            var rnd = new Random();
            rnd.NextBytes(_byteArray);
            _byteArray2 = new byte[Length];
            Buffer.BlockCopy(_byteArray, 0, _byteArray2, 0, Length);
        }

        [Benchmark(Baseline = true)]
        public bool NormalComparison()
        {
            for (var i = 0; i < Length; i++)
            {
                if (_byteArray[i] != _byteArray2[i])
                    return false;
            }
            return true;
        }

        [Benchmark()]
        public bool MemoryMarshallingComparison()
        {
            var buffer01 = MemoryMarshal.Cast<byte, long>(_byteArray);
            var buffer02 = MemoryMarshal.Cast<byte, long>(_byteArray2);
            for (var i = 0; i < buffer01.Length; i++)
            {
                if (buffer01[i] != buffer02[i])
                    return false;
            }
            return true;
        }


        [Benchmark()]
        public bool VectorComparison()
        {
            var vector01 = new Vector<byte>(_byteArray);
            var vector02 = new Vector<byte>(_byteArray2);
            return Vector.EqualsAll(vector01, vector02);
        }
    }
}
