using Lind.DDD.CachingDataSet;
using System;
namespace SOA.Service
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService
    {
        [Caching(CachingMethod.Remove, "GetUser", "GetUserPages")]
        void Add(SOA.DTO.RequestUserInfo request);
        [Caching(CachingMethod.Remove, "GetUser", "GetUserPages")]
        void Delete(string id);
        [Caching(CachingMethod.Remove, "GetUser", "GetUserPages")]
        void Update(SOA.DTO.RequestUserInfo request);

        [Caching(CachingMethod.Get)]
        System.Collections.Generic.IEnumerable<SOA.DTO.ResponseUserInfo> GetUser();
        [Caching(CachingMethod.Get)]
        System.Collections.Generic.IEnumerable<SOA.DTO.ResponseUserInfo> GetUser(SOA.DTO.RequestUserInfo request);
        [Caching(CachingMethod.Get)]
        SOA.DTO.ResponseUserInfo GetUser(string id);
        [Caching(CachingMethod.Get)]
        Lind.DDD.Paging.PagedList<SOA.DTO.ResponseUserInfo> GetUserPages(SOA.DTO.RequestUserInfo request);

    }
}
