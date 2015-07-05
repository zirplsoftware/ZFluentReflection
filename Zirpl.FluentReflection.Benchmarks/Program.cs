using System;
using System.Diagnostics;
using System.Reflection;

namespace Zirpl.FluentReflection.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            const int iterations = 100000;

            Console.WriteLine("FluentReflection benchmarks");

            Console.WriteLine("1) Setting 10 different properties {0:n0} times without reflection", iterations);
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
            RunTest(5, iterations, action);

            Console.WriteLine("2) Setting a string property {0:n0} times with standard reflection", iterations);
            action = () =>
            {
                var namesList = new[]
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
            };
            RunTest(5, iterations, action);

            Console.WriteLine("3) Setting a string property {0:n0} times with fluent reflection", iterations);
            action = () =>
            {
                var namesList = new[]
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
                    .OfAccessibility(b => b.Public())
                    .OfScope(b => b.Instance())
                    .Named(b => b.AnyIgnoreCase(namesList))
                    .Result();
                foreach (var propertyInfo in properties)
                {
                    propertyInfo.SetValue(mock, Guid.NewGuid().ToString());
                }
            };
            RunTest(5, iterations, action);



            Console.WriteLine("4) Setting a string property {0:n0} times with fluent reflection using caching", iterations);
            typeof(Mock).QueryProperties()
                    .OfAccessibility(b => b.Public())
                    .OfScope(b => b.Instance())
                .Named(b => b.AnyIgnoreCase(new []
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
                }))
                .CacheResultTo("test123");
            action = () =>
            {
                
                var mock = new Mock();
                var type = typeof(Mock);
                foreach (var propertyInfo in type.QueryProperties().FromCache("test123").Result())
                {
                    propertyInfo.SetValue(mock, Guid.NewGuid().ToString());
                }
            };
            RunTest(5, iterations, action);

            Console.WriteLine("5) Setting a string property {0:n0} times with fluent reflection using the Property extension method", iterations);
            action = () =>
            {
                var namesList = new[]
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
                foreach (var name in namesList)
                {
                    mock.Property<String>(name).Value = Guid.NewGuid().ToString();
                }
            };
            RunTest(5, iterations, action);

            Console.WriteLine();
            Console.WriteLine("Complete. Hit any key to quit");
            Console.ReadKey();
        }
        private static void RunTest(int runs, int iterations, Action action)
        {
            for (var runIndex = 0; runIndex < runs; runIndex++)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                var ts = stopWatch.Elapsed;

                Console.WriteLine("Run # {3} of {4}: {0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds, runIndex + 1, runs);
            }
        }
    }
}
