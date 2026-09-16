using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Service
{
    public class PricingTierService : IPricingTierService
    {
        private readonly IPricingTierRepository _repo;

        public PricingTierService(IPricingTierRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.PricingTier> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
