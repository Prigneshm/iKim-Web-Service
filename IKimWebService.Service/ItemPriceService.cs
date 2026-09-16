using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class ItemPriceService : IItemPriceService
    {
        private readonly IItemPriceRepository _repo;

        public ItemPriceService(IItemPriceRepository repo)
        {
            _repo = repo;
        }

        public Domain.ItemPrice Upsert(Domain.ItemPrice mItemPrice)
        {
            return _repo.Upsert(mItemPrice);
        }

        public Domain.ItemPrice Get(int id)
        {
            return _repo.Get(id);
        }
    }
}
