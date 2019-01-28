using System;
using System.Collections.Generic;
using DTC.OEM.EDF.ServiceMonitor.Logic.Actions;
using DTC.OEM.EDF.ServiceMonitor.Logic.Interfaces;
using DTC.OEM.EDF.ServiceMonitor.Monitor.Jobs;
using Microsoft.Win32.TaskScheduler;
using Quartz;

namespace DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.PingWinTaskMonitorBehavior
{
    #region Aliases

    using AsyncTask = System.Threading.Tasks.Task;
    using Task = Microsoft.Win32.TaskScheduler.Task;
    #endregion



    public class PingWinTaskMonitor : IMonitor
    {
        public const int MinimumInterval = 30;

        public ITaskRunner TaskRunner => new TaskRunner();

        public IScheduler Scheduler { get; protected set; }

        private bool _taskHasRun;
        public bool TaskHasRun
        {
            get
            {
                _taskHasRun = _taskHasRun || WindowsTask.State == TaskState.Running;
                return _taskHasRun;
            }
        }

        protected Task WindowsTask { get; set; }

        public event MonitorEventHandler TaskPerformed;

        public async AsyncTask EndMonitor()
        {
            await Scheduler.Shutdown().ConfigureAwait(false);
        }



        public async AsyncTask StartMonitor(IScheduler scheduler)
        {

            Scheduler = scheduler;

            await Scheduler.Start().ConfigureAwait(false);

            OnTaskPerformed(new MonitorEventArgs
            {
                Message = $@"Monitor {Scheduler.SchedulerName} Has Started."
            });

            //WindowsTask = await TaskRunner.RunTask(taskName);

        }

        public async AsyncTask ScheduleJob(string taskName, int intervalMinutes, DateTime startTime, DateTime endTime, string logDirectory, List<string> logArguments)
        {
            var identity = taskName.Replace(" ", ""); //Remove spaces

            

            var jobBuilder = JobBuilder.Create<EDFTaskJob>()
                .WithIdentity(identity, "group1")
                .SetJobData(
                    new JobDataMap
                    {
                        { "eventHandler", TaskPerformed}
                    })
                .UsingJobData("TaskName", taskName)
                .UsingJobData("logDirectory", logDirectory);
                
            for (int i = 0; i < logArguments.Count; i++)
            {
                jobBuilder = jobBuilder.UsingJobData($@"logArgument{i}", logArguments[i]);
            }

            var jobDetail = jobBuilder.Build();

            

            

            OnTaskPerformed(new MonitorEventArgs
            {
                Message = $@"Job {jobDetail.JobDataMap["TaskName"]} created."
            });

            
            //$@"0 0/{intervalMinutes}"
            ICronTrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"Every{intervalMinutes}Minutes", "group1")
                .WithCronSchedule($@"0 {startTime.Minute}/{intervalMinutes} {startTime.Hour}-{endTime.Hour} * * ?", x => x
                    .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")))
                .ForJob(identity, "group1")
                .Build() as ICronTrigger;

            OnTaskPerformed(new MonitorEventArgs
            {
                Message = $"JobTrigger for job \"{jobDetail.JobDataMap["TaskName"]}\" created."
            });

            await Scheduler?.ScheduleJob(jobDetail, trigger);

            OnTaskPerformed(new MonitorEventArgs
            {
                Message = $"Job {jobDetail.JobDataMap["TaskName"]} scheduled."
            });
        }

       

        protected virtual async void OnTaskPerformed(MonitorEventArgs e)
        {
            var handler = TaskPerformed;
            if (handler != null)
                await handler(this, e).ConfigureAwait(false);
        }

        public async AsyncTask StartMonitor(string taskName)
        {
            await AsyncTask.Run(() => { });
        }

        public void Dispose()
        {
            EndMonitor();
        }
    }
}
