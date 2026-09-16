using IKimWebService.Infrastructure.CustomException;
using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class StockItemService : IStockItemService
    {
        private readonly IStockItemRepository _repo;

        public StockItemService(IStockItemRepository repo)
        {
            _repo = repo;
        }

        public Domain.StockItemLister GetAll(Domain.StockItemLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.StockItem Upsert(Domain.StockItem mStockItem)
        {
            var mStockItems = _repo.GetBy(mStockItem.StockId);

            if (mStockItems != null && mStockItems.Count > 0 && mStockItems.Where(x => x.ItemId == mStockItem.ItemId).Count() > 0)
                throw new APIRequestFailedException("This item is already added to this stock!");

            return _repo.Upsert(mStockItem);
        }

        public Domain.StockItem Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
