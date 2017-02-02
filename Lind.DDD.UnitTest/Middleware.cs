using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Lind.DDD.UnitTest
{
    [TestClass]
    public class Middleware
    {
        [TestMethod]
        public void Push_Do()
        {

            Lind.DDD.Middleware.MiddlewareModel middle = new DDD.Middleware.MiddlewareModel();

            middle.Behavor = () =>
            {
                List<People> list = new List<People>();
                for (int i = 0; i < 10000; i++)
                    list.Add(new People
                    {
                        Id = 1,
                        Name = "zzl" + i,
                        SortNumber = i
                    });
                foreach (var item in list)
                    Console.WriteLine("大牛牛:{0}", item.Name);
            };
            Lind.DDD.Middleware.MiddlewareManager.AddBehavor(middle);
            Lind.DDD.Middleware.MiddlewareManager.DoBehavor();

        }

    }
}
