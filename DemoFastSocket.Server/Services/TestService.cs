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
    public class TestService : CommandSocketService<StringCommandInfo>
    {
        public override void OnConnected(IConnection connection)
        {
            base.OnConnected(connection);
            Console.WriteLine(connection.RemoteEndPoint.ToString() + " connected...");
            //发送到客户端
            connection.BeginSend(PacketBuilder.ToCommandLine("hello,can I help you ?(Exit:quit sys,Disp:display all cmds)"));
        }
        public override void OnException(IConnection connection, Exception ex)
        {
            base.OnException(connection, ex);
            Console.WriteLine("Error: " + ex.ToString());
        }
        public override void OnDisconnected(IConnection connection, Exception ex)
        {
            base.OnDisconnected(connection, ex);
            Console.WriteLine(connection.RemoteEndPoint.ToString() + " disconnected...");
        }
        protected override void HandleUnKnowCommand(IConnection connection, StringCommandInfo commandInfo)
        {
            commandInfo.Reply(connection, "unknow cmd:" + commandInfo.CmdName);
        }

    }

    #region 服务里的命令，由客户端发起
    /// <summary>
    /// 退出
    /// </summary>
    public sealed class ExitCommand : ICommand<StringCommandInfo>
    {
        /// <summary>
        /// 返回命令名称
        /// </summary>
        public string Name
        {
            get { return "exit"; }
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandInfo"></param>
        public void ExecuteCommand(IConnection connection, StringCommandInfo commandInfo)
        {
            connection.BeginDisconnect();//断开连接
        }
    }
    /// <summary>
    /// 显示命令清单
    /// </summary>
    public sealed class DispCommand : ICommand<StringCommandInfo>
    {
        /// <summary>
        /// 返回命令名称
        /// </summary>
        public string Name
        {
            get { return "disp"; }
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandInfo"></param>
        public void ExecuteCommand(IConnection connection, StringCommandInfo commandInfo)
        {
            Console.WriteLine(commandInfo.CmdName);
            connection.BeginSend(PacketBuilder.ToCommandLine("\n\r Exit:quit sys \n\r Disp:display all command \n\r New:create a task"));
        }
    }
    #endregion

}
