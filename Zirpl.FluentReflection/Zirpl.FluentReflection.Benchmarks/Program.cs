using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var iterations = 100000;

            Console.WriteLine("FluentReflection benchmarks");

            Console.WriteLine(String.Format("1) Setting 10 different properties {0:n0} times without reflection", iterations));
            var action = new Action(() =>
            {
                var mock = new Mock();
                mock.TestProperty1 = Guid.NewGuid().ToString();
                mock.TestProperty2 = Guid.NewGuid().ToString();
                mock.TestProperty3 = Guid.NewGuid().ToString();
                mock.TestProperty4 = Guid.NewGuid().ToString();
                mock.TestProperty5 = Guid.NewGuid().ToString();
                mock.TestProperty6 = Guid.NewGuid().ToString();
                mock.TestProperty7 = Guid.NewGuid().ToString();
                mock.TestProperty8 = Guid.NewGuid().ToString();
                mock.TestProperty9 = Guid.NewGuid().ToString();
                mock.TestProperty10 = Guid.NewGuid().ToString();
            });
            LogTime(1, 5, RunTest(iterations, action));
            LogTime(2, 5, RunTest(iterations, action));
            LogTime(3, 5, RunTest(iterations, action));
            LogTime(4, 5, RunTest(iterations, action));
            LogTime(5, 5, RunTest(iterations, action));

            Console.WriteLine(String.Format("2) Setting a string property {0:n0} times with standard reflection", iterations));
            action = new Action(() =>
            {
                var namesList = new String[]
                {
                    "TestProperty1",
                    "TestProperty2",
                    "TestProperty3",
                    "TestProperty4",
                    "TestProperty5",
                    "TestProperty6",
                    "TestProperty7",
                    "TestProperty8",
                    "TestProperty9",
                    "TestProperty10"
                };
                var mock = new Mock();
                var type = typeof (Mock);
                foreach (var name in namesList)
                {
                    type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public)
                        .SetValue(mock, Guid.NewGuid().ToString());   
                }
            });
            LogTime(1, 5, RunTest(iterations, action));
            LogTime(2, 5, RunTest(iterations, action));
            LogTime(3, 5, RunTest(iterations, action));
            LogTime(4, 5, RunTest(iterations, action));
            LogTime(5, 5, RunTest(iterations, action));

            Console.WriteLine(String.Format("3) Setting a string property {0:n0} times with fluent reflection", iterations));
            action = new Action(() =>
            {
                var namesList = new String[]
                {
                    "TestProperty1",
                    "TestProperty2",
                    "TestProperty3",
                    "TestProperty4",
                    "TestProperty5",
                    "TestProperty6",
                    "TestProperty7",
                    "TestProperty8",
                    "TestProperty9",
                    "TestProperty10"
                };
                var mock = new Mock();
                var type = typeof(Mock);
                var properties = type.QueryProperties()
                    .OfAccessibility().Public().And()
                    .OfScope().Instance().And()
                    .Named().AnyIgnoreCase(namesList)
                    .Result();
                foreach (var propertyInfo in properties)
                {
                    propertyInfo.SetValue(mock, Guid.NewGuid().ToString());
                }
            });
            LogTime(1, 5, RunTest(iterations, action));
            LogTime(2, 5, RunTest(iterations, action));
            LogTime(3, 5, RunTest(iterations, action));
            LogTime(4, 5, RunTest(iterations, action));
            LogTime(5, 5, RunTest(iterations, action));

            Console.WriteLine();
            Console.WriteLine("Complete. Hit any key to quit");
            Console.ReadKey();
        }

        private static void LogTime(int runNumber, int of, TimeSpan ts)
        {
            // Format and display the TimeSpan value. 
            Console.WriteLine(String.Format("Run # {3} of {4}: {0:00}:{1:00}.{2:000}",
                ts.Minutes, ts.Seconds,
                ts.Milliseconds,
                runNumber,
                of));
        }

        private static TimeSpan RunTest(int iterations, Action action)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            return stopWatch.Elapsed;
        }
    }
}
