using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Lind.DDD.KnockoutJS.Controllers
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class deviceList
    {
        public string title { get; set; }
        public string id { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

    public class Temp
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    public class HomeController : Controller
    {

        IEnumerable<Product> productList = new List<Product>            
        {
               new Product{ID=1,Name="test1",UnitPrice=100},
               new Product{ID=2,Name="test2",UnitPrice=150},
               new Product{ID=3,Name="test3",UnitPrice=41},
               new Product{ID=4,Name="test4",UnitPrice=3},
               new Product{ID=5,Name="test5",UnitPrice=281},
               new Product{ID=6,Name="test6",UnitPrice=91},
           };


        public ActionResult Index(string a = "zzl")
        {

            return View(productList);
        }

        /// <summary>
        /// 生成图表
        /// </summary>
        /// <param name="type"></param>
        public void Chart(string type)
        {
            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
               .AddTitle("人员流动情况")
               .AddSeries(name: "Employee"
               , chartType: string.IsNullOrWhiteSpace(type) ? "Column" : type//Column,Pie,Range,Stock,Point,Area
               , xValue: new[] { "一月份", "二月份", "三月份", "四月份", "五月份", "六月份", "七月份", "八月份", "九月份" }
               , yValues: new[] { "2", "6", "4", "5", "3", "4", "9", "2", "5" })
               .Write();
        }


        /// <summary>
        /// 通过数据生成图表
        /// </summary>
        /// <param name="type"></param>
        public void DataChart(string type)
        {
            var total = productList.Select(i => new
            {
                Name = i.Name,
                Price = i.UnitPrice
            }).ToList();//必须要ToList()操作

            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
               .AddTitle("人员流动情况")
               .AddSeries(name: "Employee"
               , chartType: string.IsNullOrWhiteSpace(type) ? "Column" : type)//Column,Pie,Range,Stock,Point,Area
               .DataBindTable(total, "Name")
               .Write();
        }
        public JsonResult GetProduct()
        {
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// 第三方的服务端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string UploadResult()
        {
            string data = "{'code':'OK','thumpImgUrl':'http://115.28.61.118/haoban/M00/00/86/cxw9dlcdtRaAWSWeAAASKO_9RD0332.jpg','originalImgUrl':'http://115.28.61.118/haoban/M00/00/86/cxw9dlcdtRWAYaJVAAAuMa1sHxw969.jpg'}";

            using (var http = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()       
                {    {"data",data}
                 });
                var response = http.PostAsync("Http://localhost:9497/home/UploadCallback", content).Result;
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;

            }

        }

        /// <summary>
        /// 可能是服务端来调用它
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadCallback(string data)
        {
            return Content(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Area()
        {
            return View();
        }

        public ActionResult GradeSubject()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }

        public ActionResult Map(int x = 25, int y = 25)
        {
            return View();
        }

        public JsonResult GetOnMap()
        {
            return Json(new List<deviceList> {
            new deviceList{id="1",longitude="113.372867",latitude="23.134274"},
            new deviceList{id="2",longitude="116.4459430000",latitude="39.6888290000"},
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOffMap()
        {
            return Json(new List<deviceList>{
            new  deviceList{id="1",longitude="113.264531",latitude="23.157003"},
            new  deviceList{id="2",longitude="113.330934",latitude="23.113401"},
        }, JsonRequestBehavior.AllowGet);
        }

    }
}
