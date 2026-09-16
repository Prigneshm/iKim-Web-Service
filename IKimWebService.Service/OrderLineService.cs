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
    public class OrderLineService : IOrderLineService
    {
        private readonly IOrderLineRepository _repo;

        public OrderLineService(IOrderLineRepository repo)
        {
            _repo = repo;
        }

        public Domain.OrderLineLister GetAll(Domain.OrderLineLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.OrderLine Upsert(Domain.OrderLine mOrderLine)
        {
            var mOrderLines = _repo.GetBy(mOrderLine.OrderId);

            if (mOrderLines != null && mOrderLines.Count > 0 && mOrderLines.Where(x => x.ItemId == mOrderLine.ItemId).Count() > 0)
                throw new APIRequestFailedException("This item is already added to this order!");

            return _repo.Upsert(mOrderLine);
        }

        public Domain.OrderLine Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
