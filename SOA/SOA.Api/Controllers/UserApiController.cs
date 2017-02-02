using Lind.DDD.Authorization.Api;
using Lind.DDD.Paging;
using SOA.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOA.Api.Controllers
{
    /// <summary>
    /// 用户模块API
    /// </summary>
    public class UserApiController : ApiController
    {

        IUserService userService;
        public UserApiController()
        {
            userService = Lind.DDD.IoC.ServiceLocator.Instance.GetService<IUserService>();
        }
        /// <summary>
        /// 公开的接口
        /// </summary>
        /// <returns></returns>
        public PagedList<SOA.DTO.ResponseUserInfo> Get()
        {
            try
            {
                return userService.GetUserPages(null);
            }
            catch (Exception ex)
            {
                Lind.DDD.Logger.LoggerFactory.Instance.Logger_Error(ex);
                throw;
            }

        }

        /// <summary>
        ///  有用户收取的接口
        /// </summary>
        /// <param name="page">pageindex,pagesize</param>
        /// <returns></returns>
        [ApiValiadateFilter]
        public PagedList<SOA.DTO.ResponseUserInfo> Get(string page)
        {
            return userService.GetUserPages(new DTO.RequestUserInfo()
            {
                Page = page
            });
        }
    }
}
