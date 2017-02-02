using Lind.DDD.SOA;
using Lind.DDD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace Lind.DDD.Authorization.Api
{
    #region 请求对象
    /// <summary>
    /// 参数对象
    /// </summary>
    [DataContractAttribute]
    public class RequestParam
    {
        public RequestParam(string name, string value)
        {
            this.ParamName = name;
            this.ParamValue = value;
        }
        [DataMemberAttribute]
        public string ParamName { get; private set; }
        [DataMemberAttribute]
        public string ParamValue { get; private set; }
    }
    /// <summary>
    /// 数据包中的实体
    /// </summary>
    [DataContractAttribute]
    public class RequestData
    {
        public RequestData()
        {
            this.HttpMethod = 0;
            this.RequestParam = new Dictionary<string, string>();
        }
        /// <summary>
        /// 本次通讯唯一标示
        /// </summary>
        [DataMemberAttribute]
        public string GuidKey { get; set; }
        /// <summary>
        /// 请求方式0:get,1:Post
        /// </summary>
        public int HttpMethod { get; set; }
        /// <summary>
        /// 要调用的方法
        /// </summary>
        [DataMemberAttribute]
        public string Url { get; set; }
        /// <summary>
        /// 方法的参数列表
        /// </summary>
        [DataMemberAttribute]
        public IDictionary<string, string> RequestParam { get; set; }
    }
    /// <summary>
    /// 请求数据包
    /// </summary>
    [DataContractAttribute]
    public class RequestDataSegment
    {
        public RequestDataSegment()
        {
            this.RequestData = new List<RequestData>();
        }
        [DataMemberAttribute]
        public List<RequestData> RequestData { get; set; }
    }
    #endregion

    #region 响应对象,返回主体为Lind.DDD.DTO.ReponseMessage

    /// <summary>
    /// 响应数据包
    /// </summary>
    [DataContractAttribute]
    public class ResponseDataSegment
    {
        public ResponseDataSegment()
        {
            this.ResponseData = new List<ResponseMessage>();
        }
        [DataMemberAttribute]
        public List<ResponseMessage> ResponseData { get; set; }
    }
    #endregion

    /// <summary>
    /// Api代理过滤器(api多任务请求的入口)，它通常是一个固定的api地址
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiProxyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var responseDataSegment = new ResponseDataSegment();

            var context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];//获取传统context
            var Request = context.Request;//定义传统request对象


            var query = Request.QueryString["dataSeg"];
            RequestDataSegment data = new RequestDataSegment();
            if (query != null)
                data = SerializeMemoryHelper.DeserializeFromJson<RequestDataSegment>(query);

            if (data != null && data.RequestData.Any())
            {
                foreach (var item in data.RequestData)
                {
                    try
                    {
                        HttpResponseMessage response;
                        var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                        using (var http = new HttpClient(handler))
                        {

                            if (item.HttpMethod == 0)
                            {
                                if (item.RequestParam != null)
                                {
                                    item.Url += "?";
                                    foreach (var p in item.RequestParam)
                                        item.Url += p.Key + "=" + p.Value + "&";
                                    item.Url = item.Url.Remove(item.Url.Length - 1, 1);
                                }
                                response = http.GetAsync(item.Url).Result;
                            }
                            else
                            {
                                var content = new FormUrlEncodedContent(item.RequestParam);
                                response = http.PostAsync(item.Url, content).Result;
                            }

                            response.EnsureSuccessStatusCode();
                            responseDataSegment.ResponseData.Add(new ResponseMessage
                            {
                                GuidKey = item.GuidKey,
                                Status = 200,
                                Result = response.Content.ReadAsStringAsync().Result
                            });
                        }
                    }
                    catch (Exception ex)
                    {

                        responseDataSegment.ResponseData.Add(new ResponseMessage
                        {
                            GuidKey = item.GuidKey,
                            Status = 100,
                            Result = ex.Message
                        });
                    }

                }

            }
            actionContext.Response = new HttpResponseMessage { Content = new StringContent(SerializeMemoryHelper.SerializeToJson(responseDataSegment), Encoding.GetEncoding("UTF-8"), "application/json") };
            base.OnActionExecuting(actionContext);
        }

    }
}