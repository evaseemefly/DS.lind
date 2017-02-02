using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lind.DDD.Api.Models;
using Lind.DDD.Utils;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;
using System.Text;
using Lind.DDD.SOA;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Lind.DDD.Authorization.Api;
using System.Net.Http.Headers;
using System.IO;
using Lind.DDD.Upload;
using Lind.DDD.Filters;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Lind.DDD.Api.Controllers
{
    public class TestRestfulController : Controller
    {
        public string UriAddress
        {
            get
            {
                return "http://" + Request.Url.Authority + "/api/restful/";
            }
        }
        public ActionResult Index(int page = 1)
        {


            NameValueCollection nv = new NameValueCollection();
            //读取资料
            string body = HttpHelper.Get(UriAddress, nv)
                                    .Content.ReadAsStringAsync()
                                    .Result;

            var linq = SerializeMemoryHelper.DeserializeFromJson<ResponseMessage>(body);
            var model = (linq.Result).FromPagedListJson<Models.TestApiModel>();
            return View(model);
        }
        //
        // GET: /TestRestful/Details/5
        public ActionResult Details(int id)
        {
            string body = HttpHelper.Get(UriAddress + id).Content.ReadAsStringAsync().Result;
            var returns = SerializeMemoryHelper.DeserializeFromJson<ResponseMessage>(body);
            var model = SerializeMemoryHelper.DeserializeFromJson<Models.TestApiModel>(returns.Result);
            return View(model);
        }
        //
        // GET: /TestRestful/Create
        public ActionResult Create()
        {
            List<Category> model = new List<Category>();
            model.Add(new Category
            {
                Address = new Address
                {
                    City = "beijing",
                    Province = "beijing",
                    District = "liangxiang"

                },
                Title = "zzl"
            });
            model.Add(new Category
            {
                Address = new Address
                {
                    City = "tianjing",
                    Province = "tianjing",
                    District = "binhai"

                },
                Title = "zhz"
            });


            var entity = new Category
            {
                Address = new Address
                {
                    City = "beijing",
                    Province = "beijing",
                    District = "liangxiang"

                },
                Title = "zzl"
            };
            return View();
        }
        //
        // POST: /TestRestful/Create
        [HttpPost]
        [ActionLoggerAttribute("添加API操作")]
        public ActionResult Create(TestApiModel entity)
        {
            try
            {
                //发post请求
                var response = HttpHelper.Post(UriAddress, entity.ToNameValueCollection());

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "添加资料出现问题," + response.StatusCode);
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            string body = HttpHelper.Get(UriAddress + id).Content.ReadAsStringAsync().Result;
            var returns = SerializeMemoryHelper.DeserializeFromJson<ResponseMessage>(body);
            var model = SerializeMemoryHelper.DeserializeFromJson<Models.TestApiModel>(returns.Result);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, TestApiModel entity)
        {
            try
            {
                var response = HttpHelper.Put<TestApiModel>(UriAddress + id, entity);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "添加资料出现问题");
                    return View();
                }

            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            HttpHelper.Delete(UriAddress + id);
            return RedirectToAction("Index");
        }

        public ActionResult Upload()
        {
            return View();
        }
    }
}
