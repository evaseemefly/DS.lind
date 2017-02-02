using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lind.DDD.CQRS
{

    /// <summary>
    /// 命令总线
    /// </summary>
    public class CommandBus
    {
        /// <summary>
        /// 发送到ES
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        public static void Send<T>(T command) where T : Command
        {
            var handlers = Lind.DDD.Utils.AssemblyHelper.GetTypesByInterfaces(typeof(ICommandHandler<T>));
            foreach (var item in handlers)
            {
                var handler = Lind.DDD.LindPlugins.PluginManager.Resolve<ICommandHandler<T>>(item.FullName);
                handler.Execute(command);
            }

        }
    }
}
