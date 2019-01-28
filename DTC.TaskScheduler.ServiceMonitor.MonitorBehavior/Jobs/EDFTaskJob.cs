using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTC.OEM.EDF.ServiceMonitor.Logic.Actions;
using DTC.OEM.EDF.ServiceMonitor.Logic.Interfaces;
using DTC.OEM.EDF.ServiceMonitor.Logic.Jobs;
using Quartz;

namespace DTC.OEM.EDF.ServiceMonitor.Monitor.Jobs
{
    // ReSharper disable once InconsistentNaming
    internal class EDFTaskJob : IEDFTaskJob
    {
        public ITaskRunner TaskRunner { get; protected set; }

        

        public async Task Execute(IJobExecutionContext context)
        {
            var taskName = context.MergedJobDataMap.GetString("TaskName");
            var logDirectory = context.MergedJobDataMap.GetString("LogDirectory");
            

            var logArguments = GetLogArguments(context.MergedJobDataMap);

            var winTask = await TaskRunner.RunTask(taskName, logDirectory, logArguments);
            var @event = context.MergedJobDataMap["eventHandler"] as MonitorEventHandler;
            @event?.Invoke(this, new MonitorEventArgs{Message = $"Task {winTask.Name} started at {DateTime.Now.ToShortDateString()} + {DateTime.Now.ToShortTimeString()}"});
        }

        private List<string> GetLogArguments(JobDataMap map)
        {
            var logBoxedArguments = map.Where(pair => pair.Key.ToUpper().Contains("LOGARGUMENT"))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                .Values.ToList();

            return logBoxedArguments.ConvertAll(obj => obj.ToString());
        }

        public EDFTaskJob()
        {
            //Todo Dependency Injection
            TaskRunner = new TaskRunner();
        }
    }
}