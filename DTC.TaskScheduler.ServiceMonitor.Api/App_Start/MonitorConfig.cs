using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior;
using Quartz;
using Quartz.Impl;
using Unity;

namespace DTC.TaskScheduler.ServiceMonitor.Api.App_Start
{


    public static class MonitorConfig
    {
        private static ISchedulerFactory _factory = new StdSchedulerFactory();
        private static IScheduler _scheduler;
        public static async Task Register(
            IEnumerable<Action> actions = default(IEnumerable<Action>))
        {
            _scheduler = await _factory.GetScheduler().ConfigureAwait(false);
            

            if ((actions?.Any()).GetValueOrDefault())
                foreach (var action in actions)
                {
                    action();
                }

            try
            {
                var container =
                    GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUnityContainer)) as
                        IUnityContainer;
                container.RegisterInstance(_scheduler);

                var monitor = container.Resolve<IMonitor>();

                monitor.TaskPerformed -= OnTaskPerformed;
                monitor.TaskPerformed += OnTaskPerformed;

                var taskName = ConfigurationManager.AppSettings["TaskName"];
                var interval = int.Parse(ConfigurationManager.AppSettings["IntervalMinutes"]);
                var start = DateTime.Parse(ConfigurationManager.AppSettings["StartTime"]);
                var end = DateTime.Parse(ConfigurationManager.AppSettings["EndTime"]);
                if (end < start)
                {
                    end = end.AddDays(1);
                }
                var address1 = ConfigurationManager.AppSettings["IPAddress1"];

                var coll = new List<string>();
                coll.Add(address1);


                 
                await monitor.StartMonitor(_scheduler);


                await monitor.ScheduleJob(taskName, interval, start, end, coll);
            }
            catch
            {
                ;
            }
        }

        private static Task OnTaskPerformed(object sender, MonitorEventArgs e)
        {
            return Task.Delay(1);
        }
    }
}