using Lind.DDD.Utils;
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
    /// sum command
    /// 用于将一组int32数字求和并返回
    /// </summary>
    public sealed class InsertCommand : ICommand<DSSBinaryCommandInfo>
    {
        /// <summary>
        /// 返回服务名称
        /// </summary>
        public string Name
        {
            get { return "UserInsert"; }
        }
        /// <summary>
        /// 执行命令并返回结果
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandInfo"></param>
        public void ExecuteCommand(IConnection connection, DSSBinaryCommandInfo commandInfo)
        {
            if (commandInfo.Buffer == null || commandInfo.Buffer.Length == 0)
            {
                Console.WriteLine("UserInsert参数为空");
                connection.BeginDisconnect();
                return;
            }

            var entity = SerializeMemoryHelper.DeserializeFromBinary(commandInfo.Buffer);
            string str = "result:1,versonNumber:" + commandInfo.VersionNumber
                + ",extProperty:" + commandInfo.ExtProperty
                + ",projectName:" + commandInfo.ProjectName
                + ",message:" + entity.ToString();
            Console.WriteLine(str);
            commandInfo.Reply(connection, Encoding.UTF8.GetBytes(str));//返回到客户端...
        }


    }

}
