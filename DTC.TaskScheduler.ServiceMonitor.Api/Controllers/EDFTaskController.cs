using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
//using DTC.TaskScheduler.ServiceMonitor.Api.Models;
using DTC.TaskScheduler.ServiceMonitor.Logic;
using Quartz;
using Quartz.Impl;

namespace DTC.TaskScheduler.ServiceMonitor.Api.Controllers
{
    [System.Web.Mvc.RoutePrefix("api")]
    public class EdfTaskController : ApiController
    {
        
        private readonly IScheduler _scheduler;
        private ISimpleTrigger jobTrigger;
        private IJobDetail job;
        private readonly ITaskRunner _taskRunner;

        public EdfTaskController(IScheduler scheduler, ITaskRunner runner)
        {
            _scheduler = scheduler;
            _taskRunner = runner;
        }

        


        [Route("EdfTask/Rerun/{scheduledTime}"), HttpGet]
        public async Task<IHttpActionResult> RerunEdfTask([FromUri]string scheduledTime)
        {
            try
            {
                //DateTime scheduled; //= DateTime.Now;
                //scheduledTime = DateTime.Now.AddSeconds(5).ToString();
                //DateTime.TryParse(scheduledTime, out scheduled);

                

                job = JobBuilder.Create<MonitorBehaviorJob>()
                    .WithIdentity("TestTask", "group1")
                    .UsingJobData("Name", "Test Task")
                    .Build();

                jobTrigger = (TriggerBuilder.Create()
                        .WithIdentity("SimpleTrigger", "group1")
                        .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow))
                        .Build())
                    as ISimpleTrigger;

                if (_scheduler.IsStarted)
                {
                    
                    await _scheduler.UnscheduleJob(jobTrigger.Key);
                }
                else
                {

                    await _scheduler.Start();

                }




                //DateTimeOffset scheduled = DateTimeOffset.UtcNow.AddSeconds(5);

                


                var runtime = await _scheduler.ScheduleJob(job, jobTrigger);
                //var localRunTime = runtime.
                Debug.WriteLine($"The task will run at {runtime}");

                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }



    }
}