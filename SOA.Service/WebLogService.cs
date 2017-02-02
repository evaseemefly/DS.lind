using Lind.DDD.IoC;
using Lind.DDD.IRepositories;
using SOA.DTO;
using SOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA.Service
{
    public class WebLogService
    {
        IMongoRepository<WebLog> userMongoRepository = ServiceLocator.Instance.GetService<IMongoRepository<WebLog>>();

        public void Add(WebLog request)
        {
            userMongoRepository.Insert(request);
        }

        public IEnumerable<ResponseWebLog> Get(RequestWebLog request)
        {
            return userMongoRepository.GetModel(i => i.Type == request.Type).MapTo<ResponseWebLog>();
        }
    }
}
