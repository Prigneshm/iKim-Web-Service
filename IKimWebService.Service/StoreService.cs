using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repo;

        public StoreService(IStoreRepository repo)
        {
            _repo = repo;
        }

        public Domain.StoreLister GetAll(Domain.StoreLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Store Upsert(Domain.Store mStore)
        {
            return _repo.Upsert(mStore);
        }

        public Domain.Store Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Store> GetAllParentStore()
        {
            return _repo.GetAllParentStore();
        }

        public List<Domain.Store> GetActiveStore()
        {
            return _repo.GetActiveStore();
        }


    }
}
