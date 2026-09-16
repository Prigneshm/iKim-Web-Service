using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IRepository
{
    public interface IStockRepository
    {
        Domain.StockLister GetAll(Domain.StockLister mLister);

        Domain.Stock Upsert(Domain.Stock mStock);

        Domain.Stock Get(int id);

        //List<Domain.Stock> GetBy(int storeId);

        void Delete(int id, int loginUserId);

        List<Domain.Stock> GetActiveStock();
    }
}
