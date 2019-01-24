using System.Threading.Tasks;
using DTC.TaskScheduler.ServiceMonitor.Logic;
using DTC.TaskScheduler.ServiceMonitor.Logic.Actions;
using DTC.TaskScheduler.ServiceMonitor.Logic.Jobs;
using Quartz;

namespace DTC.TaskScheduler.ServiceMonitor.MonitorBehavior.Jobs
{
    internal class EDFTaskJob : IEDFTaskJob
    {
        public ITaskRunner TaskRunner { get; protected set; }

        public async Task Execute(IJobExecutionContext context)
        {
            await TaskRunner.RunTask(context.MergedJobDataMap.GetString("TaskName"));
        }

        public EDFTaskJob()
        {
            //Todo Dependency Injection
            TaskRunner = new TaskRunner();
        }
    }
}