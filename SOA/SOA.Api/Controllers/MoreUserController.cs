using Lind.DDD.SOA;
using SOA.DTO;
using SOA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace SOA.Api.Controllers
{
    /// <summary>
    /// 批量添加
    /// </summary>
    public class MoreUserController : ApiController
    {
        /// <summary>
        /// 批量添加
        /// [FromBody]FormDataCollection value
        /// </summary>
        /// <param name="value"></param>
        public void Post([ModelBinder]RequestUserInfo[] value)
        {

        }

        public ResponseMessage Get([FromUri]RequestWebLog request)
        {
            return new ResponseMessage(request.ContainFields)
            {
                Body = new WebLogService().Get(request),
                Status = 100
            };
        }
    }
}
