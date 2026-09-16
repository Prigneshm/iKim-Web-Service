using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public Domain.OrderLister GetAll(Domain.OrderLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Order Upsert(Domain.Order mOrder)
        {
            return _repo.Upsert(mOrder);
        }

        public Domain.Order Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
