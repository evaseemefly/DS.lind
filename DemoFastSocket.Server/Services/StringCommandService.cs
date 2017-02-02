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
    public class StringCommandService : CommandSocketService<AsyncBinaryCommandInfo>
    {
        public override void OnConnected(IConnection connection)
        {
            Console.WriteLine("connection...");
            base.OnConnected(connection);
        }
        protected override void HandleUnKnowCommand(IConnection connection, AsyncBinaryCommandInfo commandInfo)
        {
            Console.WriteLine(commandInfo.CmdName);
            Console.WriteLine(commandInfo.SeqID);
            Console.WriteLine("命令不被认出...");
        }
    }
}
