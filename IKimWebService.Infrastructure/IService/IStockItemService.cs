using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IService
{
    public interface IStockItemService
    {
        Domain.StockItemLister GetAll(Domain.StockItemLister mLister);
        Domain.StockItem Upsert(Domain.StockItem mStockItem);
        Domain.StockItem Get(int id);
        void Delete(int id, int loginUserId);
    }
}
