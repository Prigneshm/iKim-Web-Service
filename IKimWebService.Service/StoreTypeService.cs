using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class StoreTypeService : IStoreTypeService
    {
        private readonly IStoreTypeRepository _repo;

        public StoreTypeService(IStoreTypeRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.StoreType> GetAll()
        {
            return _repo.GetAll();
        }
        
    }
}
