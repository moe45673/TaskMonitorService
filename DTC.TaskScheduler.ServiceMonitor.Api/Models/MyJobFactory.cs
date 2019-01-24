using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz.Simpl;

namespace DTC.TaskScheduler.ServiceMonitor.Api.Models
{
    public class MyJobFactory : SimpleJobFactory
    {

        public MyJobFactory() : base()
        {
            ;
        }

    }
}