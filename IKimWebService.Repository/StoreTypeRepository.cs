using IKimWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKimWebService.Repository
{
    public class StoreTypeRepository : IStoreTypeRepository
    {
        public List<Domain.StoreType> GetAll()
        {
            var mStoreType = new List<Domain.StoreType>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStoreType = (from st in context.StoreTypes
                                       where st.IsActive
                                       select new
                                       {
                                           Id = st.Id,
                                           Name = st.Name,
                                           Description = st.Description,
                                           IsActive = st.IsActive,
                                       }).AsEnumerable();

                    mStoreType = JsonSerializer.Deserialize<List<Domain.StoreType>>(JsonSerializer.Serialize(efStoreType.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mStoreType;
        }
    }
}
