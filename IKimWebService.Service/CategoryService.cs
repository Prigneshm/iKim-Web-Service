using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public Domain.CategoryLister GetAll(Domain.CategoryLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Category Upsert(Domain.Category mCategory)
        {
            return _repo.Upsert(mCategory);
        }

        public Domain.Category Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
        public List<Domain.Category> GetActiveCategory()
        {
            return _repo.GetActiveCategory();
        }
    }
}
