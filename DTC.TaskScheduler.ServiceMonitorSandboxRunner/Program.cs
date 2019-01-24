using DTC.TaskScheduler.ServiceMonitor.Logic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTC.TaskScheduler.ServiceMonitorSandboxRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static async Task MainAsync(string[] args)
        {
            await InnerMainAsync(args);
        }

        public static async Task InnerMainAsync(string[] args)
        {
            var tEvent = new TaskRunner();
                var Message = await tEvent.RunTask("Test Task");

            Console.WriteLine(Message.ToString());
        }
    }
}
