using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;
        private readonly IItemPriceRepository _priceRepository;

        public ItemService(IItemRepository repo, IItemPriceRepository priceRepository)
        {
            _repo = repo;
            _priceRepository = priceRepository;
        }

        public Domain.ItemLister GetAll(Domain.ItemLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Item Upsert(Domain.Item mItem)
        {
            mItem = _repo.Upsert(mItem);

            if (mItem.Id != 0)
            {
                foreach (var itemPrice in mItem.Prices)
                {
                    itemPrice.ItemId = mItem.Id;
                    _priceRepository.Upsert(itemPrice);
                }
            }

            return mItem;
        }

        public Domain.Item Get(int id)
        {
            var mItem = _repo.Get(id);

            mItem.Prices = _priceRepository.GetByItemId(id);

            return mItem;
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Item> GetActiveItem()
        {
            return _repo.GetActiveItem();
        }
    }
}
