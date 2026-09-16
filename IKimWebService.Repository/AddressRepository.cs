using IKimWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKimWebService.Repository
{
    public class AddressRepository : IAddressRepository
    {
        public Domain.Address Upsert(Domain.Address mAddress)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efAddress = context.Addresses.FirstOrDefault(x => x.Id == mAddress.Id);
                    if (efAddress == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mAddress.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efAddress = new Persistence.Address
                        {
                            AuditId = audit.Id
                        };
                        context.Addresses.Add(efAddress);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efAddress.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mAddress.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efAddress.Line1 = mAddress.Line1;
                    efAddress.Line2 = mAddress.Line2;
                    efAddress.Line3 = mAddress.Line3;
                    efAddress.City = mAddress.City;
                    efAddress.State = mAddress.State;
                    efAddress.ZipCode = mAddress.ZipCode;

                    context.SaveChanges();
                    mAddress.Id = efAddress.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }

        public Domain.Address Get(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efAddress = (from c in context.Addresses
                                     where c.Id == id
                                     select new
                                     {
                                         Id = c.Id,
                                         Line1 = c.Line1,
                                         Line2 = c.Line2,
                                         Line3 = c.Line3,
                                         City = c.City,
                                         State = c.State,
                                         ZipCode = c.ZipCode,
                                     }).FirstOrDefault();

                    if (efAddress != null)
                    {
                        mAddress = JsonSerializer.Deserialize<Domain.Address>(JsonSerializer.Serialize(efAddress));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }
    }
}
