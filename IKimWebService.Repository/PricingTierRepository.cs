using IKimWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKimWebService.Repository
{
    public class PricingTierRepository : IPricingTierRepository
    {
        public List<Domain.PricingTier> GetAll()
        {
            var mPricingTier = new List<Domain.PricingTier>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efPricingTier = (from pt in context.PricingTiers
                                      where pt.IsActive
                                      select new
                                      {
                                          Id = pt.Id,
                                          Name = pt.Name,
                                          Description = pt.Description,
                                          IsActive = pt.IsActive,
                                      }).AsEnumerable();

                    mPricingTier = JsonSerializer.Deserialize<List<Domain.PricingTier>>(JsonSerializer.Serialize(efPricingTier.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPricingTier;
        }
    }
}
