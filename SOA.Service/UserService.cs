using Lind.DDD.IoC;
using Lind.DDD.IRepositories;
using Lind.DDD.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOA.DTO;
using Lind.DDD.SOA;
using System.Reflection.Emit;
using AutoMapper;
using Lind.DDD.Paging;
namespace SOA.Service
{
    /// <summary>
    /// 用户模块的业务
    /// </summary>
    public class UserService : SOA.Service.IUserService
    {
        IRepository<UserInfo> userRepository = ServiceLocator.Instance.GetService<IRepository<UserInfo>>();

        public ResponseUserInfo GetUser(string id)
        {
            int _id = Convert.ToInt32(id);
            return userRepository.GetModel()
                                 .Where(i => i.Id == _id)
                                 .MapTo<DTO.ResponseUserInfo>()
                                 .FirstOrDefault();

        }
        public IEnumerable<ResponseUserInfo> GetUser()
        {
            return userRepository.GetModel()
                                 .MapTo<ResponseUserInfo>();

        }
        public IEnumerable<ResponseUserInfo> GetUser(RequestUserInfo request)
        {

            return userRepository.GetModel()
                                 .MapTo<DTO.ResponseUserInfo>();
        }

        public PagedList<ResponseUserInfo> GetUserPages(RequestUserInfo request)
        {
            request = request ?? new RequestUserInfo();

            var sort = request.GetSortDictionary();

            var linq = userRepository.GetModel()
                                     .OrderByDescending(i => i.Id)
                                     .ToPagedList(request.GetPageParameters())
                                     .MapToPaged<DTO.ResponseUserInfo>();
            return linq;
        }
        public void Add(RequestUserInfo request)
        {
            var entity = request.MapTo<UserInfo>();
            userRepository.Insert(entity);
        }
        public void Update(RequestUserInfo request)
        {
            var entity = userRepository.GetModel().FirstOrDefault(i => i.Id == Convert.ToInt32(request.Id));
            request.MapTo<UserInfo>(entity);
            userRepository.Update(entity);
        }
        public void Delete(string id)
        {
            userRepository.Delete(new UserInfo { Id = Convert.ToInt32(id) });
        }
    }
}
