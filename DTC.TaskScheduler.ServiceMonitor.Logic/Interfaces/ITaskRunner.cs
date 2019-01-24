using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Microsoft.Win32.TaskScheduler.Task;

// ReSharper disable once CheckNamespace
namespace DTC.TaskScheduler.ServiceMonitor.Logic
{
    using AsyncWindowsTask = Task<Task>;
    public interface ITaskRunner
    {
        AsyncWindowsTask RunTask(string taskName, string logDirectory, List<string> args);
        AsyncWindowsTask RunTask(string taskName);



    }
}
