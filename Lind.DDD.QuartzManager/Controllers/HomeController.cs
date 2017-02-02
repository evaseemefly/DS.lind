using CrystalQuartz.Core.SchedulerProviders;
using Lind.DDD.QuartzManager;
using Lind.DDD.Utils;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
namespace MvcShell.Controllers
{
    /// <summary>
    /// corn复杂的quartz配置
    /// </summary>
    public class HomeController : Controller
    {
        private static RemoteSchedulerProvider remoteSchedulerProvider;
        private string _filePath = System.Configuration.ConfigurationManager.AppSettings["quartzCronXmlPath"]
            ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quartz_jobs.xml");
        private XDocument _doc;
        private IEnumerable<XElement> _listJob;
        private IEnumerable<XElement> _listTrigger;
        private static object lockObj = new object();

        static HomeController()
        {
            remoteSchedulerProvider = new CrystalQuartz.Core.SchedulerProviders.RemoteSchedulerProvider();
            remoteSchedulerProvider.SchedulerHost = System.Configuration.ConfigurationManager.AppSettings["SchedulerHost"]
                ?? "tcp://127.0.0.1:667/QuartzScheduler";

        }
        public HomeController()
        {
            _doc = XDocument.Load(_filePath);
            _listJob = _doc.Root.Elements("{http://quartznet.sourceforge.net/JobSchedulingData}schedule").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}job");
            _listTrigger = _doc.Root.Elements("{http://quartznet.sourceforge.net/JobSchedulingData}schedule").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}trigger");

        }
        public ContentResult TestXml()
        {

            string xsd = AppDomain.CurrentDomain.BaseDirectory + "test.xsd";

            string xml = AppDomain.CurrentDomain.BaseDirectory + "test.xml";

            var result = XmlHelper.ValidateXml(xsd, xml);

            return Content(result.IsComplete ? "结束" : result.GetMessage());
        }

        public ActionResult Index()
        {
            try
            {
                var linq = from d1 in _listJob
                           join d2 in _listTrigger
                           on
                           d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value.Trim()
                           equals
                           d2.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-name").Value.Trim()
                           select new QuartzCronModel
                           {
                               JobName = d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value,
                               JobGroup = d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}group").Value,
                               Dll = d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-type").Value,
                               IsFromToTime = d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map") == null ? string.Empty : d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}value").ToList()[0].Value,
                               OccurTimeRegionFrom = d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map") == null ? string.Empty : d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}value").ToList()[1].Value,
                               OccurTimeRegionTo = d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map") == null ? string.Empty : d1.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}value").ToList()[2].Value,
                               CronExpression = d2.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron-expression").Value,
                               TriggerName = d2.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value,
                               TriggerGroup = d2.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}group").Value,
                               RunStatus = remoteSchedulerProvider.Scheduler.GetTriggerState(new TriggerKey(d2.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value, d2.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}group").Value))
                           };
                return View(linq.ToList());
            }
            catch (Exception)
            {

                return View();
            }
        }

        public ActionResult Edit(string jobName)
        {
            var job = _listJob.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value.Trim() == jobName.Trim()).First();
            var trigger = _listTrigger.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-name").Value.Trim() == jobName.Trim()).First();
            return View(new QuartzCronModel
            {
                JobName = job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value,
                JobGroup = job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}group").Value,
                Dll = job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-type").Value,
                IsFromToTime = job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map") == null ? string.Empty : job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}value").ToList()[0].Value,
                OccurTimeRegionFrom = job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map") == null ? string.Empty : job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}value").ToList()[1].Value,
                OccurTimeRegionTo = job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map") == null ? string.Empty : job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}value").ToList()[2].Value,
                CronExpression = trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron-expression").Value,
                TriggerName = trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value,
                TriggerGroup = trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}group").Value,

            });

        }

        [HttpPost]
        public ActionResult Edit(QuartzCronModel entity)
        {
            entity.IsFromToTime = Request.Form["IsFromToTime"];
            var job = _listJob.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value.Trim() == entity.JobName.Trim()).First();
            var trigger = _listTrigger.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-name").Value.Trim() == entity.JobName.Trim()).First();

            job.SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}group", entity.JobGroup);
            job.SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}job-type", entity.Dll);
            if (!string.IsNullOrWhiteSpace(entity.IsFromToTime) &&
                !string.IsNullOrWhiteSpace(entity.OccurTimeRegionFrom) &&
                !string.IsNullOrWhiteSpace(entity.OccurTimeRegionTo))
            {
                job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").ToList()[0].SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}value", entity.IsFromToTime);
                job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").ToList()[1].SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}value", entity.OccurTimeRegionFrom);
                job.Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map").Elements("{http://quartznet.sourceforge.net/JobSchedulingData}entry").ToList()[2].SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}value", entity.OccurTimeRegionTo);
            }
            trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}cron-expression", entity.CronExpression.Trim());
            trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}name", entity.TriggerName.Trim());
            trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}group", entity.TriggerGroup.Trim());
            trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}job-name", entity.JobName.Trim());
            trigger.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron").SetElementValue("{http://quartznet.sourceforge.net/JobSchedulingData}job-group", entity.JobGroup.Trim());

            lock (lockObj)
            {
                _doc.Save(_filePath);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string jobName)
        {
            var job = _listJob.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value.Trim() == jobName.Trim()).First();
            job.Remove();
            var trigger = _listTrigger.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}cron")
                .Element("{http://quartznet.sourceforge.net/JobSchedulingData}job-name").Value.Trim() == jobName.Trim())
                .First();
            trigger.Remove();
            lock (lockObj)
            {
                _doc.Save(_filePath);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuartzCronModel entity)
        {
            entity.IsFromToTime = Request.Form["IsFromToTime"];

            if (_listJob.Where(i => i.Element("{http://quartznet.sourceforge.net/JobSchedulingData}name").Value == entity.JobName).Count() > 0)
            {
                ModelState.AddModelError("", "这个job名称已经存在了，请使用其它名称！");
                return View();
            }

            if (ModelState.IsValid)
            {
                XElement item = _doc.Root.Element("{http://quartznet.sourceforge.net/JobSchedulingData}schedule");
                var job = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}job");
                job.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}name", entity.JobName));
                job.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}group", entity.JobGroup));
                job.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}job-type", entity.Dll));
                if (!string.IsNullOrWhiteSpace(entity.IsFromToTime) &&
                    !string.IsNullOrWhiteSpace(entity.OccurTimeRegionFrom) &&
                    !string.IsNullOrWhiteSpace(entity.OccurTimeRegionTo))
                {
                    var jobDataMap = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}job-data-map");
                    var entry1 = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}entry");
                    entry1.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}key", "IsFromToTime"));
                    entry1.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}value", entity.IsFromToTime));
                    jobDataMap.Add(entry1);
                    entry1 = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}entry");
                    entry1.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}key", "OccurTimeRegionFrom"));
                    entry1.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}value", entity.OccurTimeRegionFrom));
                    jobDataMap.Add(entry1);
                    entry1 = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}entry");
                    entry1.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}key", "OccurTimeRegionTo"));
                    entry1.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}value", entity.OccurTimeRegionTo));
                    jobDataMap.Add(entry1);

                    job.Add(jobDataMap);
                }
                item.Add(job);
                var trigger = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}trigger");
                var cron = new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}cron");
                cron.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}cron-expression", entity.CronExpression));
                cron.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}name", entity.TriggerName));
                cron.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}group", entity.TriggerGroup));
                cron.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}job-name", entity.JobName));
                cron.Add(new XElement("{http://quartznet.sourceforge.net/JobSchedulingData}job-group", entity.JobGroup));
                trigger.Add(cron);
                item.Add(trigger);
                lock (lockObj)
                {
                    _doc.Save(_filePath);
                }
            }
            else
            {
                ModelState.AddModelError("", "请认真填写表单！");
                return View();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 恢复开始任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public ActionResult Start(string jobName, string group)
        {
            remoteSchedulerProvider.Scheduler.ResumeJob(new JobKey(jobName, group));
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public ActionResult Stop(string jobName, string group)
        {
            remoteSchedulerProvider.Scheduler.PauseJob(new JobKey(jobName, group));
            return RedirectToAction("Index");
        }

        public ActionResult JobList()
        {
            //运行中的job
            var scheduler = remoteSchedulerProvider.Scheduler.GetJobGroupNames();
            var executingJobs = remoteSchedulerProvider.Scheduler.GetCurrentlyExecutingJobs();
            List<QuartzCronModel> JobTrigger = new List<QuartzCronModel>();

            foreach (var group in scheduler)
            {
                foreach (var sub in remoteSchedulerProvider.Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group)))
                {
                    var job = remoteSchedulerProvider.Scheduler.GetJobDetail(sub);
                    var trigger = remoteSchedulerProvider.Scheduler.GetTriggersOfJob(sub).First();
                    var status = remoteSchedulerProvider.Scheduler.GetTriggerState(new TriggerKey(trigger.Key.Name));

                    JobTrigger.Add(new QuartzCronModel
                    {
                        JobName = job.Key.Name,
                        JobGroup = job.Key.Group,
                        TriggerName = trigger.Key.Name,
                        TriggerGroup = trigger.Key.Name,
                        RunStatus = remoteSchedulerProvider.Scheduler.GetTriggerState(new TriggerKey(trigger.Key.Name, trigger.Key.Group)),
                        Dll = job.JobType.Name,
                        CronExpression = (trigger as ICronTrigger).CronExpressionString
                    });
                }
            }
            return View(JobTrigger);
        }
        public ActionResult UpdateCron(string jobName, string jobGroup, string cronExpression)
        {
            JobKey jobKey = new JobKey(jobName, jobGroup);
            IJobDetail job = remoteSchedulerProvider.Scheduler.GetJobDetail(jobKey);
            ITrigger trigger = remoteSchedulerProvider.Scheduler.GetTriggersOfJob(jobKey).First();
            (trigger as ICronTrigger).CronExpressionString = cronExpression;
            Quartz.Collection.ISet<ITrigger> triggersForJob = new Quartz.Collection.HashSet<ITrigger> { trigger };
            remoteSchedulerProvider.Scheduler.ScheduleJob(job, triggersForJob, true);

            return RedirectToAction("JobList");
        }

    }


}
