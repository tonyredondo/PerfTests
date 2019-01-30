using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace PerfTests
{
    [CoreJob(baseline: true)]
    [RPlotExporter, RankColumn, MinColumn, MaxColumn]
    public class Test02
    {
        private ClassWithFactory _classWithFactory = new ClassWithFactory(() => new object());
        private ClassWithStructFactory<ObjectFactory> _classWithStructFactory = new ClassWithStructFactory<ObjectFactory>();

        [GlobalSetup]
        public void Setup()
        {

        }

        [Benchmark(Baseline = true)]
        public object NormalClassWithFactory()
        {
            return _classWithFactory.CreateNew();
        }

        [Benchmark]
        public object NormalClassWithStructFactory()
        {
            return _classWithStructFactory.CreateNew();
        }


        #region Test Types
        public class ClassWithFactory
        {
            private Func<object> _newDelegate;
            
            public ClassWithFactory(Func<object> newDelegate)
            {
                _newDelegate = newDelegate;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public object CreateNew()
            {
                return _newDelegate();
            }
        }

        //

        public interface IFactory
        {
            object New();
        }
        public class ClassWithStructFactory<T> where T: struct, IFactory
        {
            private T _factory = default;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public object CreateNew()
            {
                return _factory.New();
            }
        }

        public readonly struct ObjectFactory : IFactory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public object New()
            {
                return new object();
            }
        }
        #endregion

    }
}
