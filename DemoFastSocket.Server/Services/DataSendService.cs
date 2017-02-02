using Lind.DDD.FastSocket.Server;
using Lind.DDD.FastSocket.Server.Command;
using Lind.DDD.FastSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFastSocket.Server
{
    /// <summary>
    /// 数据操作的服务
    /// </summary>
    public class DataSendService : CommandSocketService<DSSBinaryCommandInfo>
    {
        /// <summary>
        /// 当连接时会调用此方法
        /// </summary>
        /// <param name="connection"></param>
        public override void OnConnected(IConnection connection)
        {
            base.OnConnected(connection);
            Console.WriteLine(connection.RemoteEndPoint.ToString() + " connected");
        }

        protected override void HandleUnKnowCommand(IConnection connection, DSSBinaryCommandInfo commandInfo)
        {
            
            
            Console.WriteLine("客户输入的方法名称不被服务器端知道...");
        }
    }

}
