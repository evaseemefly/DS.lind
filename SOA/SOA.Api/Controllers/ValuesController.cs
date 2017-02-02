using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SOA.Service;
using Lind.DDD.SOA;
using SOA.DTO;
using Lind.DDD.HttpExtensions.HttpResponse;
using Lind.DDD.Utils;
using Lind.DDD.Authorization;
using Lind.DDD.Authorization.Api;
namespace SOA.Api.Controllers
{

    public class ValuesController : ApiController
    {
        IUserService userService = Lind.DDD.IoC.ServiceLocator.Instance.GetService<IUserService>();
        // GET api/values/id
        public ResponseMessage Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("参数不能小于0");

            var dto = userService.GetUser(id);
            return new ResponseMessage()
            {
                Body = dto,
            };
        }

        public ResponseMessage Get(int b)
        {
            return new ResponseMessage()
           {
               Body = "OK",
           };
        }
        public ResponseMessage Get(int a, int b)
        {
            var linq = userService.GetUser();
            return new ResponseMessage()
            {
                Body = linq,
            };
        }
        // GET api/values/
        public ResponseMessage Get(string zzl, [FromUri] RequestUserInfo request)
        {
            var result = new ResponseMessage(request.ContainFields);

            if (request.IsValid)
            {
                result.Body = userService.GetUser(request);
            }
            result.ErrorMessage = request.GetRuleViolationMessages();
            result.Status = 200;
            return result;
        }

        // GET api/values/
        public ResponseMessage Get([FromUri] RequestUserInfo request)
        {
            var result = new ResponseMessage(request.ContainFields);

            if (request.IsValid)
            {
                result.Body = userService.GetUserPages(request);
            }
            result.ErrorMessage = request.GetRuleViolationMessages();
            result.Status = 200;
            return result;
        }

        // POST api/values
        public void Post([FromBody]RequestUserInfo value)
        {
            //建立用户的事件被发布了，所有订阅了本事件的客户端将被触发
            Lind.DDD.PublishSubscribe.PubSubManager.Instance.Publish("CreateUser", value.UserName);
            userService.Add(value);
        }

        // PUT api/values/5
        public void Put(string id, [FromBody]RequestUserInfo value)
        {
            userService.Update(value);
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            userService.Delete(id);
        }

    }
}