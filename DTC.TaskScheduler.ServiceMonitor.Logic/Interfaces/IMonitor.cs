using System;
using System.Collections.Generic;
using Quartz;


// ReSharper disable once CheckNamespace
namespace DTC.OEM.EDF.ServiceMonitor.Logic.Interfaces
{
    #region Aliases
    using AsyncTask = System.Threading.Tasks.Task;
    #endregion

    public delegate AsyncTask MonitorEventHandler(object sender, MonitorEventArgs e);

    

    public interface IMonitor : IDisposable
    {
        AsyncTask StartMonitor(string taskName);
        AsyncTask StartMonitor(IScheduler scheduler);

        AsyncTask ScheduleJob(string taskName, int intervalMinutes, DateTime startTime, DateTime endTime,
            string logDirectory, List<string> logArguments);
        AsyncTask EndMonitor();

        event MonitorEventHandler TaskPerformed;

        bool TaskHasRun { get; }

        ITaskRunner TaskRunner { get; }
    }

    public class MonitorEventArgs : EventArgs
    {
        
        public string Message { get; set; }
        

    }
}
