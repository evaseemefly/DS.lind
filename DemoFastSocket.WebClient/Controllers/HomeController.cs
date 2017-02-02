using Lind.DDD.FastSocket.Client;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DemoFastSocket.WebClient.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 从WEB页面发FastSocket请求
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //注册服务器节点，这里可注册多个(name不能重复）
            var client = new DSSBinarySocketClient(8192, 8192, 3000, 3000);

            client.RegisterServerNode("127.0.0.1:8403", new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 8403));
            client.Send("UserInsert", 1, "zzl", 1, "test", SerializeMemoryHelper.SerializeToBinary("hello web world!"), res => res.Buffer).ContinueWith(c =>
            {
                if (c.IsFaulted)
                {
                    throw c.Exception;
                }
                Console.WriteLine(Encoding.UTF8.GetString(c.Result));

            });
            ViewBag.msg = "消息发送了";

            return View();

        }

    }
}
