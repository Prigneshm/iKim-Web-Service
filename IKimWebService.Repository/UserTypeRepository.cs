using IKimWebService.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace IKimWebService.Repository
{
    public class UserTypeRepository : IUserTypeRepository
    {
        public List<Domain.UserType> GetAll()
        {
            var mUserType = new List<Domain.UserType>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efUserType = (from ut in context.UserTypes
                                      where ut.IsActive
                                      select new
                                      {
                                          Id = ut.Id,
                                          Name = ut.Name,
                                      }).AsEnumerable();

                    mUserType = JsonSerializer.Deserialize<List<Domain.UserType>>(JsonSerializer.Serialize(efUserType.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mUserType;
        }
    }
}
