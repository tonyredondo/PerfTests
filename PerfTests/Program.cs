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
                    case "/test01":
                        selectedTest = typeof(Test01);
                        break;
                    case "/test02":
                        selectedTest = typeof(Test02);
                        break;
                    case "/test03":
                        selectedTest = typeof(Test03);
                        break;
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
