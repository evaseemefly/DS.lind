using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.DDD.Events;
namespace Lind.DDD.Events.Demo
{
    public class UserEvent : EventBase
    {
        public int UserId { get; set; }
    }
}
