using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS.Events
{
    public class OrderConfirmEvent : EventBase
    {
        public Guid OrderId { get; set; }
        public DateTime ConfirmDate { get; set; }
        public int UserId { get; set; }
    }
}
