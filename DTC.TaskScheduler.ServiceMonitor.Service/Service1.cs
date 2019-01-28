using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using DTC.TaskScheduler.ServiceMonitor.Monitor;
using DTC.TaskScheduler.ServiceMonitor.Monitor.PingWinTaskMonitor;
using Quartz;
using Quartz.Impl;

namespace DTC.TaskScheduler.ServiceMonitor.Service
{
    public partial class Service1 : ServiceBase
    {
        private ISchedulerFactory _factory = new StdSchedulerFactory();
        private IScheduler _scheduler;
        private IMonitor _monitor;
        private EventLog eventLog;
        public Service1()
        {
            //InitializeComponent();

            
        }

        protected override void OnStart(string[] args)
        {
            InitializeEventLog();
            OnStartAsync();
        }

        protected override void OnStop()
        {

        }

        public async Task OnStartAsync()
        {
            _scheduler = await _factory.GetScheduler().ConfigureAwait(false);
            _monitor = new PingWinTaskMonitor();

            _monitor.TaskPerformed -= OnTaskPerformed;
            _monitor.TaskPerformed += OnTaskPerformed;

            var EDF1TaskName = ConfigurationManager.AppSettings["EDFTask1"];
            var EDF2TaskName = ConfigurationManager.AppSettings["EDFTask2"];
            var EDF1LogDirectory = ConfigurationManager.AppSettings["logDirectory1"];
            var EDF2LogDirectory = ConfigurationManager.AppSettings["logDirectory2"];
            var interval = int.Parse(ConfigurationManager.AppSettings["IntervalMinutes"]);
            var oemStartInterval = int.Parse(ConfigurationManager.AppSettings["MultipleOemIntervalMinutes"]);
            var start1 = DateTime.Parse(ConfigurationManager.AppSettings["StartTime"]);
            var end1 = DateTime.Parse(ConfigurationManager.AppSettings["EndTime"]);
            if (end1 < start1)
            {
                end1 = end1.AddDays(1);
            }

            var start2 = start1.AddMinutes(oemStartInterval);
            var end2 = end1;


            var logArguments = new List<string> { ConfigurationManager.AppSettings["logArgument1"], ConfigurationManager.AppSettings["logArgument2"] };

        

            await _monitor.StartMonitor(_scheduler);

            await _monitor.ScheduleJob(EDF1TaskName, interval, start1, end1, EDF1LogDirectory, logArguments);
            await _monitor.ScheduleJob(EDF2TaskName, interval, start2, end2, EDF2LogDirectory, logArguments);

        }

        private void InitializeEventLog()
        {
            //eventLog = new EventLog();
            //if (!EventLog.SourceExists(this.ServiceName))
            //{
            //    EventLog.CreateEventSource(this.ServiceName, this.ServiceName + "Log");
            //}
            //eventLog.Source = this.ServiceName;
            ////eventLog.Log = this.ServiceName + "Log";

            ////bool log_errors_in_event_log = Util.GetValueFromAppSettings("log_errors_in_event_log", log_errors_in_event_log_default_value);
            ////if (log_errors_in_event_log)
            //    Log.EventLog = eventLog;
        }

        private async Task OnTaskPerformed(object sender, MonitorEventArgs e)
        {
            await Task.Delay(1);
        }
    }

//    #region OutputLog
//    public static class Log
//    {
//        public static DTLoggerClassic logger = new DTLoggerClassic() { MessageType = "recoverablemessage" };

//        public static string ApplicationProfileId { get; set; }

//        public static string LenderId { get; set; }

//        public static System.Diagnostics.EventLog EventLog = null;

//        public static void LogError(Exception ex)
//        {
//            try
//            {
//                string machineName = System.Environment.MachineName;
//                string logQueueName = GetValueFromAppSettings("log_queue_name", string.Empty);
//                logger.LogEvent("queue",
//                    logQueueName,
//                    null,
//                    machineName,
//                    "EDFTaskServiceMonitor",
//                    ex.ToString(),
//                    "EDFTaskServiceMonitorLog",
//                    string.Empty,
//                    null,
//                    null,
//                    null,
//                    null,
//                    null,
//                    null,
//                    GetMethodName(2),
//                    "E",
//                    "LR",
//                    LenderId,
//                    "Y",
//                    "X",
//                    null,
//                    null,
//                    ApplicationProfileId);
//            }
//            catch (Exception)
//            {
//                //cannot log this log error ... so just do nothing
//            }
//        }

//        private static string GetValueFromAppSettings(string config, string defaultValue)
//        {
//            string result = defaultValue;
//            string fromConfig = ConfigurationManager.AppSettings[config];
//            if (!string.IsNullOrWhiteSpace(fromConfig))
//                result = fromConfig;
//            return result;
//        }

//        private static string GetMethodName(int frameIndex)
//        {
//            var result = string.Empty;
//            StackTrace stackTrace = new StackTrace();

//            if (stackTrace.FrameCount > frameIndex)
//            {
//                StackFrame stackFrame = stackTrace.GetFrame(frameIndex);
//                MethodBase methodBase = stackFrame.GetMethod();

//                if (methodBase.DeclaringType != null)
//                {
//                    result = methodBase.DeclaringType.Name + "." + methodBase.Name;
//                }
//            }
//            return result;
//        }
//    }
//#endregion
}
