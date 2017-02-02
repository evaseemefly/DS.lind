using Lind.DDD.FastSocket.Server;
using Lind.DDD.FastSocket.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFastSocket.Server
{

    class Program
    {
        static void Main(string[] args)
        {
            Lind.DDD.LindQueue.BrokerManager.Start();

        }
    }
}
