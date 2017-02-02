using Lind.DDD.Aspects;
using Lind.DDD.EntityValidation;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lind.DDD.Domain
{
    /// <summary>
    /// 领域模型，实体模型基类，它可能有多种持久化方式，如DB,File,Redis,Mongodb,XML等
    /// Lind.DDD框架的领域模型与数据库实体合二为一
    /// </summary>
    [PropertyChangedAttribute]
    public abstract class EntityBase : ContextBoundObject, IEntity, INotifyPropertyChanged
    {
        /// <summary>
        /// 实体初始化
        /// </summary>
        public EntityBase()
        {
            this.Status = Status.Normal;
            this.UpdateDateTime = DateTime.Now;
            this.CreateDateTime = DateTime.Now;
            this.PropertyChanged += EntityBase_PropertyChanged;
        }

        /// <summary>
        /// 建立时间
        /// </summary>
        [XmlIgnore, DataMember(Order = 3), XmlElement(Order = 3), DisplayName("建立时间"), Column("CreateTime"), Required]
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [XmlIgnore, DataMember(Order = 2), XmlElement(Order = 2), DisplayName("更新时间"), Column("UpdateTime"), Required]
        public DateTime UpdateDateTime { get; set; }
        /// <summary>
        /// 实体状态
        /// </summary>
        [XmlIgnore, DataMember(Order = 1), XmlElement(Order = 1), DisplayName("状态"), Required]
        public Status Status { get; set; }

        /// <summary>
        /// 拿到实体验证的结果列表
        /// 结果为null或者Enumerable.Count()==0表达验证成功
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToArray();

            foreach (var i in properties)
            {
                var attr = i.GetCustomAttributes();
                foreach (var a in attr)
                {
                    var val = (a as ValidationAttribute);
                    if (val != null)
                        if (!val.IsValid(i.GetValue(this)))
                        {
                            yield return new RuleViolation(val.ErrorMessage, i.Name);
                        }
                }
            }

        }

        #region PropertyChangedEventHandler Events
        /// <summary>
        /// 属性值变更事件，外部可以直接订阅它
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 事件实例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntityBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("基类EntityBase属性:{0},值:{1}", e.PropertyName, sender.GetType().GetProperty(e.PropertyName).GetValue(sender));
        }
        /// <summary>
        /// 触发事件,写在每个属性的set块中CallerMemberName特性表示当前块的属性名
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
