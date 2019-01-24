using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTC.TaskScheduler.ServiceMonitor.Logic.Actions;
using DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.Jobs;
using Microsoft.Win32.TaskScheduler;
using Quartz;


namespace DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.PingWinTaskMonitor
{
    using System.Net;
    using DTC.TaskScheduler.ServiceMonitor.Logic;
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

        public async AsyncTask ScheduleJob(string taskName, int intervalMinutes, DateTime startTime, DateTime endTime, List<string> IPAddresses)
        {

            IJobDetail job = JobBuilder.Create<EDFTaskJob>()
                .WithIdentity("EDFTask", "group1")
                .UsingJobData("TaskName", taskName)
                .Build();

            
            //$@"0 0/{intervalMinutes}"
            ICronTrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"Every{intervalMinutes}Minutes", "group1")
                .WithCronSchedule($@"0 {startTime.Minute}/{intervalMinutes} {startTime.Hour}-{endTime.Hour} * * ?", x => x
                    .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")))
                .ForJob("EDFTask", "group1")
                .Build() as ICronTrigger;

            await Scheduler?.ScheduleJob(job, trigger);

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
