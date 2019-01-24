using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using DTC.TaskScheduler.ServiceMonitor.Logic;
using Quartz;
using TaskState = Microsoft.Win32.TaskScheduler.TaskState;


// ReSharper disable once CheckNamespace
namespace DTC.TaskScheduler.ServiceMonitor.MonitorBehavior
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
            List<string> IPAddresses);
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
