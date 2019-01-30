using BenchmarkDotNet.Attributes;

namespace PerfTests
{
    public class Test06
    {
        private const int Length = 100_000_000;
        private TestClass _testClass = new TestClass();


        [Benchmark(OperationsPerInvoke = Length)]
        public void FieldAccess()
        {
            int value;
            for (var i = 0; i < Length; i++)
                value = _testClass.Field;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = Length)]
        public void PropertyAccess()
        {
            int value;
            for (var i = 0; i < Length; i++)
                value = _testClass.Property;
        }

        private class TestClass
        {
            public readonly int Field;
            public int Property { get; private set; }

            public TestClass()
            {
                Field = int.MaxValue;
                Property = Field;
            }
        }
    }
}
