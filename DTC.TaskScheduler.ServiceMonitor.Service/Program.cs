using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace DTC.TaskScheduler.ServiceMonitor.Service
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
