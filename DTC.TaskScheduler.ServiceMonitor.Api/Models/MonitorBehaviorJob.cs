using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DTC.TaskScheduler.ServiceMonitor.Logic.Actions;
using DTC.TaskScheduler.ServiceMonitor.Logic;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior;
using Quartz;
using Unity;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.PingWinTaskMonitorBehavior;

namespace DTC.TaskScheduler.ServiceMonitor.Api.Models
{
    public class MonitorBehaviorJob : IJob
    {
        private IMonitorBehavior _monitorBehavior;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                //TODO Utilize Container for instantiation - implement custom jobFactory
                _monitorBehavior = new PingWinTaskMonitorBehavior();


                
                await Task.Run((() => { ;}));
                //var start = DateTime.Parse()
                //await _monitorBehavior.StartMonitor(taskName, interval);
            }
            catch
            {
                ;
            }
        }

        
    }
}