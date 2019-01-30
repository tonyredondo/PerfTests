using BenchmarkDotNet.Attributes;

namespace PerfTests
{
    [CoreJob(baseline: true)]
    [RPlotExporter, RankColumn, MinColumn, MaxColumn]
    public class Test03
    {
        private const int TestCount = 10000;

        private NormalClass _normalClass = new NormalClass();
        private ClassWithoutVirtual _classWithoutVirtual = new ClassWithoutVirtual();
        private ClassWithVirtual _classWithVirtual = new ClassWithVirtual();
        private IGetData _interfaceCall = new ClassWithoutVirtual();
        private AbstractBaseClass _abstractCall = new ImplementationClass();

        [GlobalSetup]
        public void Setup()
        {

        }

        [Benchmark(Baseline = true, OperationsPerInvoke = TestCount)]
        public void NormalClassCall()
        {
            for (var i = 0; i < TestCount; i++)
                _normalClass.GetData();
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void SealedClassCall()
        {
            for (var i = 0; i < TestCount; i++)
                _classWithoutVirtual.GetData();
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void ClassWithVirtualCall()
        {
            for (var i = 0; i < TestCount; i++)
                _classWithVirtual.GetData();
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void InterfaceCall()
        {
            for (var i = 0; i < TestCount; i++)
                _interfaceCall.GetData();
        }

        [Benchmark(OperationsPerInvoke = TestCount)]
        public void AbstractCall()
        {
            for (var i = 0; i < TestCount; i++)
                _abstractCall.GetData();
        }




        //
        public class ClassWithVirtual
        {
            public virtual string GetData()
            {
                return "Hello World";
            }
        }

        //

        public class NormalClass
        {
            public string GetData()
            {
                return "Hello World";
            }
        }


        //
        public interface IGetData
        {
            string GetData();
        }

        public sealed class ClassWithoutVirtual : IGetData
        {
            public string GetData()
            {
                return "Hello World";
            }
        }

        //
        public abstract class AbstractBaseClass : IGetData
        {
            public abstract string GetData();
        }

        public sealed class ImplementationClass : AbstractBaseClass
        {
            public override string GetData()
            {
                return "Hello World";
            }
        }
    }
}
