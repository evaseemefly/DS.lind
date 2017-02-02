using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lind.DDD.QuartzManager
{
    /// <summary>
    /// Cron 触发器对象
    /// </summary>
    public class QuartzCronModel
    {
        [DisplayName("任务名称")]
        [Required]
        public string JobName { get; set; }
        [DisplayName("工作组名称")]
        [Required]
        public string JobGroup { get; set; }
        [DisplayName("任务程序集")]
        [Required]
        public string Dll { get; set; }
        [DisplayName("触发器名称")]
        [Required]
        public string TriggerName { get; set; }
        [DisplayName("触发器工作组")]
        [Required]
        public string TriggerGroup { get; set; }
        [DisplayName("Cron表达式")]
        [Required]
        public string CronExpression { get; set; }
        [DisplayName("运行状态")]
        [ReadOnly(true)]
        public TriggerState RunStatus { get; set; }
        [DisplayName("是否启用时间段统计")]
        [ReadOnly(true)]
        public string IsFromToTime { get; set; }
        [DisplayName("时间段统计开始时间")]
        public string OccurTimeRegionFrom { get; set; }
        [DisplayName("时间段统计结束时间")]
        public string OccurTimeRegionTo { get; set; }
    }
    /// <summary>
    /// 自定义数据类型
    /// </summary>
    public class JobDataMap
    {
        public Dictionary<string, string> Entry { get; set; }
    }
}