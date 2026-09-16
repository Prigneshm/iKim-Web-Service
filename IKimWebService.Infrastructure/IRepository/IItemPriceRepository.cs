using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IRepository
{
    public interface IItemPriceRepository
    {
        Domain.ItemPrice Upsert(Domain.ItemPrice mItemPrice);
        Domain.ItemPrice Get(int id);
        List<Domain.ItemPrice> GetByItemId(int itemId);
    }
}
