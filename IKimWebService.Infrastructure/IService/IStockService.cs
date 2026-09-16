using System.Collections.Generic;

namespace IKimWebService.Infrastructure.IService
{
    public interface IStockService
    {
        Domain.StockLister GetAll(Domain.StockLister mLister);

        Domain.Stock Upsert(Domain.Stock mStock);

        Domain.Stock Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Stock> GetActiveStock();
    }
}
