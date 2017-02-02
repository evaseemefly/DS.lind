using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lind.DDD.Aspects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Lind.DDD.LindPlugins;

namespace Lind.DDD.UnitTest
{
    public interface IAopHelloTest2 :
        Lind.DDD.Aspects.IAspectProxy
    {
        string Hello(string title, int age);
    }
    public class AopHello : IAopHelloTest2
    {
        #region IHello 成员
        [CachingAspect(CachingMethod.Get)]
        public string Hello(string title, int age)
        {
            //beforeAspect
            Console.WriteLine("您好，大叔!");
            //..
            //..
            //afterAspect
            return "OK";
        }
        #endregion
    }
    [TestClass]
    public class Aop
    {
        [TestMethod]
        public void TestMethod1()
        {
            ITest test = ProxyFactory.CreateProxy(typeof(ITest), typeof(LoggerAspectAttribute)) as ITest;
            test.Do();
        }

        [TestMethod]
        public void AutoPlug()
        {
            var old = PluginManager.Resolve<IAopHelloTest2>("Lind.DDD.UnitTest.AopHello");
            var result = old.Hello("zz", 1);
            Console.WriteLine(result);
        }

    }
}
