using BenchmarkDotNet.Running;
using System;

namespace PerfTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Type selectedTest = null;
            if (args != null && args.Length > 0)
            {
                switch(args[0])
                {
                    default:
                        selectedTest = typeof(Test01);
                        break;
                }
            }
            else
                selectedTest = typeof(Test01);

            var summary = BenchmarkRunner.Run(selectedTest);
        }
    }
}
