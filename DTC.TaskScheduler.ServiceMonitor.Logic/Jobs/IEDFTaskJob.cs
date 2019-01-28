using DTC.OEM.EDF.ServiceMonitor.Logic.Interfaces;
using Quartz;

namespace DTC.OEM.EDF.ServiceMonitor.Logic.Jobs
{
    // ReSharper disable once InconsistentNaming
    public interface IEDFTaskJob : IJob
    {

        ITaskRunner TaskRunner { get; }
       
    }

   
}
