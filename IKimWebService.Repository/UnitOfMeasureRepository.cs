using IKimWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKimWebService.Repository
{
    public class UnitOfMeasureRepository : IUnitOfMeasureRepository
    {
        public List<Domain.UnitOfMeasure> GetAll()
        {
            var mUnitOfMeasure = new List<Domain.UnitOfMeasure>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efPricingTier = (from um in context.UnitOfMeasures
                                         where um.IsActive
                                         select new
                                         {
                                             Id = um.Id,
                                             Name = um.Name,
                                             Description = um.Description,
                                             Value = um.Value,
                                             IsActive = um.IsActive,
                                         }).AsEnumerable();

                    mUnitOfMeasure = JsonSerializer.Deserialize<List<Domain.UnitOfMeasure>>(JsonSerializer.Serialize(efPricingTier.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mUnitOfMeasure;
        }
    }
}
