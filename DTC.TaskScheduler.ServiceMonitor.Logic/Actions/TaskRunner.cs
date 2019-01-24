using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTC.TaskScheduler.ServiceMonitor.Logic;
using Microsoft.Win32.TaskScheduler;
using Quartz.Util;
using AsyncTaskReturnWinTask = System.Threading.Tasks.Task<Microsoft.Win32.TaskScheduler.Task>;
using AsyncTask = System.Threading.Tasks.Task;
using Task = Microsoft.Win32.TaskScheduler.Task;


namespace DTC.TaskScheduler.ServiceMonitor.Logic.Actions
{
    public class TaskRunner : ITaskRunner
    {
        
        public AsyncTaskReturnWinTask RunTask(string taskName)
        {
            return RunTask(taskName, string.Empty, new List<string>());
        }

        public AsyncTaskReturnWinTask RunTask(string taskName, string logDirectory, List<string> args)
        {
            return AsyncTask.Run(() =>
            {
                var taskResult = default(Task);

                try
                {

                    using (var ts = new TaskService())
                    {
                        //eventLog.WriteEntry("Start MyOtherApp", EventLogEntryType.Information, 1337);
                        var edfTask = ts.FindTask(taskName);

                        

                        if (edfTask?.State != TaskState.Running)
                        {

                            var isSuccessful = Validate(logDirectory, args);

                            if (!isSuccessful)
                            {
                                edfTask?.Run();
                            }
                        }
                        taskResult = edfTask;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                return taskResult;
            });
        }

        #region ValidationLogic
        private bool Validate(string logPath, List<string> successCollection, bool onlyCheckTodaysLogs = true)
        {

            if (logPath.IsNullOrWhiteSpace())
            {
                return false;
            }
            var logDirectory = new DirectoryInfo(logPath);
            var lastLog = logDirectory.GetFiles().
                OrderByDescending(f => f.CreationTime).
                FirstOrDefault();

            return IsLastLoggedRunSuccess(lastLog, successCollection, onlyCheckTodaysLogs);
        }

        private bool IsLastLoggedRunSuccess(FileInfo lastLog, IEnumerable<string> successArguments, bool onlyCheckTodaysLogs = true)
        {



            

            if (onlyCheckTodaysLogs &&
                !string.Equals(
                    lastLog?.CreationTime.ToShortDateString(),
                    DateTime.Now.ToShortDateString()
                    )
                )
            {
                return false;
            }

            return LogIndicatesSuccess(lastLog, successArguments);

        }

        private bool LogIndicatesSuccess(FileInfo logFile, IEnumerable<string> logArguments)
        {
            var logSuccessful = true;


            try
            {
                using (var sr = logFile.OpenText())
                {
                    var logContents = sr.ReadToEnd();
                    foreach (var arg in logArguments)
                    {
                        logSuccessful = logSuccessful && logContents.Contains(arg);
                    }
                }
            }
            catch (Exception e)
            {
                ;
            }


            return logSuccessful;
        }
        #endregion
    }
}
