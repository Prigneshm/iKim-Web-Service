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
    public class StockService : IStockService
    {
        private readonly IStockRepository _repo;

        public StockService(IStockRepository repo)
        {
            _repo = repo;
        }

        public Domain.StockLister GetAll(Domain.StockLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Stock Upsert(Domain.Stock mStock)
        {
            //var mStocks = _repo.GetBy(mStock.StoreId);

            //if (mStocks != null && mStocks.Count > 0 && mStocks.Where(x => x.StoreId == mStock.StoreId).Count() > 0)
            //    throw new APIRequestFailedException("This item is already added to this stock!");

            //return _repo.Upsert(mStock);

            return _repo.Upsert(mStock);
        }

        public Domain.Stock Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Stock> GetActiveStock()
        {
            return _repo.GetActiveStock();
        }
    }
}
