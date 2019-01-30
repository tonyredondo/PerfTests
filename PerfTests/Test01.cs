using BenchmarkDotNet.Attributes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PerfTests
{
    [CoreJob(baseline: true)]
    [RPlotExporter, RankColumn, MinColumn, MaxColumn, MemoryDiagnoser]
    public class Test01
    {
        private const int MaxItems = 1000;
        private const int TestCount = 10000;
        private MemoryStream ms = null;
        private int[] ValuesArray = null;

        [GlobalSetup]
        public void Setup()
        {
            ms = new MemoryStream();
            var rnd = new Random();
            ValuesArray = new int[MaxItems];
            for (var i = 0; i < MaxItems; i++)
                ValuesArray[i] = rnd.Next();

            //Warm up
            SimpleImplementation();
            var resMethod01 = ms.ToArray();
            SimpleImplementationWithBuffer();
            var resMethod02 = ms.ToArray();
            StackllocBufferImplementation();
            var resMethod03 = ms.ToArray();
            MemoryMarshallingImplementation();
            var resMethod04 = ms.ToArray();
            PointerImplementation();
            var resMethod05 = ms.ToArray();

            Console.WriteLine("Is SimpleImplementationWithBuffer producing same result as SimpleImplementation: " + resMethod01.SequenceEqual(resMethod02));
            Console.WriteLine("Is StackllocBufferImplementation producing same result as SimpleImplementation: " + resMethod01.SequenceEqual(resMethod03));
            Console.WriteLine("Is MemoryMarshallingImplementation producing same result as SimpleImplementation: " + resMethod01.SequenceEqual(resMethod04));
            Console.WriteLine("Is PointerImplementation producing same result as SimpleImplementation: " + resMethod01.SequenceEqual(resMethod05));
            Console.WriteLine();
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = TestCount)]
        public void SimpleImplementation()
        {
            var values = ValuesArray;
            ms.Position = 0;
            for (var i = 0; i < values.Length; i++)
            {
                var byteArray = BitConverter.GetBytes(values[i]);
                ms.Write(byteArray);
            }
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void SimpleImplementationWithBuffer()
        {
            var values = ValuesArray;
            ms.Position = 0;
            var buffer = new byte[4];
            for (var i = 0; i < values.Length; i++)
            {
                BitConverter.TryWriteBytes(buffer, values[i]);
                ms.Write(buffer);
            }
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void StackllocBufferImplementation()
        {
            var values = ValuesArray;
            ms.Position = 0;
            Span<byte> buffer = stackalloc byte[4];
            for (var i = 0; i < values.Length; i++)
            {
                BitConverter.TryWriteBytes(buffer, values[i]);
                ms.Write(buffer);
            }
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void MemoryMarshallingImplementation()
        {
            var values = ValuesArray;
            ms.Position = 0;
            var valuesInBytes = MemoryMarshal.Cast<int, byte>(values);
            ms.Write(valuesInBytes);
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public unsafe void PointerImplementation()
        {
            var values = ValuesArray;
            ms.Position = 0;
            fixed (int* valuePointer = values)
            {
                var valuesInBytes = new ReadOnlySpan<byte>((byte*)valuePointer, values.Length * 4);
                ms.Write(valuesInBytes);
            }
        }
    }
}
