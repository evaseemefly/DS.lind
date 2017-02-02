using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lind.DDD.Domain
{
    /// <summary>
    /// 属性变更拦截器
    /// </summary>
    internal class PropertyChangedProxy : RealProxy
    {
        Type serverType;
        public PropertyChangedProxy(Type serverType)
            : base(serverType)
        {
            this.serverType = serverType;
        }
        public override IMessage Invoke(IMessage msg)
        {
            //构造方法
            if (msg is IConstructionCallMessage)
            {
                IConstructionCallMessage constructCallMsg = msg as IConstructionCallMessage;
                IConstructionReturnMessage constructionReturnMessage = this.InitializeServerObject((IConstructionCallMessage)msg);
                RealProxy.SetStubData(this, constructionReturnMessage.ReturnValue);
                return constructionReturnMessage;
            }
            //其它方法（属性也是方法,它会被翻译成set_property,get_property,类似于java里的属性封装）
            else if (msg is IMethodCallMessage)
            {

                IMethodCallMessage callMsg = msg as IMethodCallMessage;
                object[] args = callMsg.Args;
                IMessage message;
                try
                {

                    if (callMsg.MethodName.StartsWith("set_") && args.Length == 1)
                    {
                        string propertyName = Regex.Split(callMsg.MethodName, "set_")[1];
                        //这里检测到是set方法，然后应怎么调用对象的其它方法呢？
                        var method = this.serverType.GetMethod("OnPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (method != null)
                        {
                            var obj = GetUnwrappedServer();
                            obj.GetType().GetProperty(propertyName).SetValue(obj, args.FirstOrDefault());
                            method.Invoke(obj, new object[] { propertyName });//这块对象为空了
                        }

                    }

                    object o = callMsg.MethodBase.Invoke(GetUnwrappedServer(), args);
                    message = new ReturnMessage(o, args, args.Length, callMsg.LogicalCallContext, callMsg);
                }

                catch (Exception e)
                {

                    message = new ReturnMessage(e, callMsg);

                }

                return message;

            }

            return msg;

        }
    }
}
