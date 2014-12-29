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
            var iterations = 10000000;

            Console.WriteLine("FluentReflection benchmarks");

            Console.WriteLine(String.Format("1) Setting a string property {0:n0} times without reflection", iterations));
            var action = new Action(() => new Mock().TestProperty = Guid.NewGuid().ToString());
            LogTime(RunTest(iterations, action));

            Console.WriteLine(String.Format("2) Setting a string property {0:n0} times with standard reflection", iterations));
            action = new Action(() => typeof(Mock).GetProperty("TestProperty", BindingFlags.Instance | BindingFlags.Public).SetValue(new Mock(), Guid.NewGuid().ToString()));
            LogTime(RunTest(iterations, action));

            Console.WriteLine(String.Format("3) Setting a string property {0:n0} times with fluent reflection", iterations));
            action = new Action(() => typeof(Mock).QueryProperties().OfAccessibility().Public().And().Named().Exactly("TestProperty").ExecuteSingle().SetValue(new Mock(), Guid.NewGuid().ToString()));
            LogTime(RunTest(iterations, action));

            Console.WriteLine();
            Console.WriteLine("Complete. Hit any key to quit");
            Console.ReadKey();
        }

        private static void LogTime(TimeSpan ts)
        {
            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
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
