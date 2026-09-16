using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IService
{
    public interface IStoreService
    {
        Domain.StoreLister GetAll(Domain.StoreLister mLister);
        Domain.Store Upsert(Domain.Store mStore);
        Domain.Store Get(int id);
        void Delete(int id, int loginUserId);
        List<Domain.Store> GetAllParentStore();
        List<Domain.Store> GetActiveStore();
    }
}
