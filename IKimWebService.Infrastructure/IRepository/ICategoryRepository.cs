using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IRepository
{
    public interface ICategoryRepository
    {
        Domain.CategoryLister GetAll(Domain.CategoryLister mLister);
        Domain.Category Upsert(Domain.Category mCategory);
        Domain.Category Get(int id);
        void Delete(int id, int loginUserId);
        List<Domain.Category> GetActiveCategory();
    }
}
