using System.Collections.Generic;
using System.Threading.Tasks;
using Task = Microsoft.Win32.TaskScheduler.Task;

// ReSharper disable once CheckNamespace
namespace DTC.OEM.EDF.ServiceMonitor.Logic.Interfaces
{
    using AsyncWindowsTask = Task<Task>;
    public interface ITaskRunner
    {
        AsyncWindowsTask RunTask(string taskName, string logDirectory, List<string> args);
        AsyncWindowsTask RunTask(string taskName);



    }
}
