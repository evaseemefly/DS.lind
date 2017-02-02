using Lind.DDD.MongoDbClient;
using MongoDB.Bson;
using MongoDB.Driver;
using SOA.DTO;
using SOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA.Service
{
    public class UserServiceMongo : IUserService
    {
        #region IUserService 成员

        public void Add(DTO.RequestUserInfo request)
        {
            var entity = request.MapTo<UserInfo>();
            MongoManager<UserInfo>.Instance.InsertOne(entity);
        }

        public void Delete(string id)
        {
            MongoManager<UserInfo>.Instance.DeleteOne(Builders<UserInfo>.Filter.Eq(i => i.Id, id));
        }

        public IEnumerable<DTO.ResponseUserInfo> GetUser()
        {
            return MongoManager<UserInfo>.Instance
                                       .Find<UserInfo>(i => true)
                                       .ToList()
                                       .MapTo<ResponseUserInfo>();
        }

        public IEnumerable<DTO.ResponseUserInfo> GetUser(DTO.RequestUserInfo request)
        {

            return MongoManager<UserInfo>.Instance
                                        .Find(Builders<UserInfo>.Filter.Eq(i => i.Id, request.Id))
                                        .ToList()
                                        .MapTo<ResponseUserInfo>();
        }

        public DTO.ResponseUserInfo GetUser(string id)
        {
            return MongoManager<UserInfo>.Instance
                                        .Find(Builders<UserInfo>.Filter.Eq(i => i.Id, id))
                                        .FirstOrDefault()
                                        .MapTo<ResponseUserInfo>();

        }

        public Lind.DDD.Paging.PagedList<DTO.ResponseUserInfo> GetUserPages(DTO.RequestUserInfo request)
        {
            //  var query = new QueryDocument(request.GetProperyiesDictionary());
            return null;
        }

        public void Update(DTO.RequestUserInfo request)
        {
            //var entity = request.MapTo<UserInfo>();
            //var document = new UpdateDocument(entity.GetProperyiesDictionary());
            //var filter = Builders<UserInfo>.Filter.Eq(s => s.Id, request.Id);
            //var update = Builders<UserInfo>.Update.Set(s => s.Description, description);
            //MongoManager<UserInfo>.Instance
            //                      .Update(Query.EQ("_id", ObjectId.Parse(request.Id)), document);
        }

        #endregion
    }
}
