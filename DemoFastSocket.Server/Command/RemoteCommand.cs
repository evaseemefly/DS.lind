using DemoFastSocket.Models;
using Lind.DDD.Utils;
using Lind.DDD.FastSocket.Server.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lind.DDD.FastSocket.SocketBase;

namespace DemoFastSocket.Server.Command
{

    
    /// <summary>
    /// 对于AsyncBinaryCommandInfo命令
    /// </summary>
    public class RemoteCommand : ICommand<AsyncBinaryCommandInfo>
    {
        public string Name
        {
            get { return "SendQueue"; }
        }
        public void ExecuteCommand(IConnection connection, AsyncBinaryCommandInfo commandInfo)
        {
            if (commandInfo.Buffer == null || commandInfo.Buffer.Length == 0)
            {
                Console.WriteLine("SendQueue参数为空");
                connection.BeginDisconnect();
                return;
            }

            var entity = SerializeMemoryHelper.DeserializeFromBinary(commandInfo.Buffer);
            string str = "result:1,CmdName:" + commandInfo.CmdName
                + ",SeqID:" + commandInfo.SeqID
                + ",message:" + entity.ToString();
            Console.WriteLine(str);

            var old = new User { Name = "hello world" };
            commandInfo.Reply(connection, SerializeMemoryHelper.SerializeToBinary(old));//返回到客户端..
        }


    }
}
