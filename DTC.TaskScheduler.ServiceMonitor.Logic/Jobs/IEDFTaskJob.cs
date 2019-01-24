using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace DTC.TaskScheduler.ServiceMonitor.Logic.Jobs
{
    public interface IEDFTaskJob : IJob
    {

        ITaskRunner TaskRunner { get; }
    }
}
