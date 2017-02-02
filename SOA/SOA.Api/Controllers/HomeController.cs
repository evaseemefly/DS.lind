using Lind.DDD.Authorization.Api;
using Lind.DDD.Paging;
using Newtonsoft.Json;
using SOA.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SOA.Api.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            var url = "http://localhost:24334/api/UserApi";
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                var response = http.GetAsync(url);
                string resultStr = response.Result.Content.ReadAsStringAsync().Result;
                //var obj = JsonConvert.DeserializeObject<dynamic>(resultStr);
                //foreach (var item in obj)
                //{
                //    return Content("userName:" + item.UserName);
                //}

                var model = JsonConvert.DeserializeObject<PagedList<SOA.DTO.ResponseUserInfo>>(resultStr);
                return View(model);
            }
        }

        /// <summary>
        /// 需要收取的用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserListValidate(string page = "1,5")
        {
        
         
                var param = new NameValueCollection();
                param.Add("page", page);
                param.Add("AppKey", "lind");
                var ciphertext = ApiValidateHelper.GenerateCipherText(param);
                var response =Lind.DDD.Utils.HttpHelper.Get("http://localhost:24334/api/UserApi",ciphertext);

                if (response.StatusCode != HttpStatusCode.OK)
                    return Content(response.ReasonPhrase);

                var model = JsonConvert.DeserializeObject<PagedList<SOA.DTO.ResponseUserInfo>>
                    (response.Content.ReadAsStringAsync().Result);

                return View("UserList", model);
       

        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(FormCollection form)
        {
            var url = form["url"];
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                //await异步等待回应
                var response = await http.GetAsync(url);
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();

                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                return Content(await response.Content.ReadAsStringAsync());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(RequestUserInfo form)
        {
            var url = "http://localhost:24334/api/values";
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                //await异步等待回应
                var response = await http.PostAsJsonAsync(url, form);
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();
                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                return RedirectToAction("List");
            }
        }

        public ActionResult Create()
        {

            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Edit(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Edit(RequestUserInfo form)
        {
            var url = "http://localhost:24334/api/values/" + form.Id;
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                //await异步等待回应
                var response = await http.PutAsJsonAsync(url, form);
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();
                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                return RedirectToAction("List");
            }
        }
        public async Task<ActionResult> Delete(string Id)
        {
            var url = "http://localhost:24334/api/values/" + Id;
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                //await异步等待回应
                var response = await http.DeleteAsync(url);
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();
                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                return RedirectToAction("List");
            }
        }

    }
}
