using System.Threading.Tasks;
using System.Web.Http;
using DTC.TaskScheduler.ServiceMonitor.Logic.Actions;
using DTC.TaskScheduler.ServiceMonitor.Logic;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.Jobs;

using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using System;
using System.Configuration;
using System.Diagnostics;
using Quartz;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.PingWinTaskMonitor;

namespace DTC.TaskScheduler.ServiceMonitor
{
    public static class UnityConfig
    {
        
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IMonitor, PingWinTaskMonitor>();
            
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            
        }

        
    }
}