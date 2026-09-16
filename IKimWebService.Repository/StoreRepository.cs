using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace IKimWebService.Repository
{
    public class StoreRepository : IStoreRepository
    {
        public Domain.StoreLister GetAll(Domain.StoreLister mLister)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var query = from s in context.Stores
                                join st in context.StoreTypes on s.StoreTypeId equals st.Id
                                join pt in context.PricingTiers on s.PricingTierId equals pt.Id
                                join at in context.AuditTrails on s.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    ContactPerson = s.ContactPerson,
                                    PhoneNumber = s.PhoneNumber,
                                    AlternativePhoneNumber = s.AlternativePhoneNumber,
                                    AddressId = s.AddressId,
                                    StoreTypeId = s.StoreTypeId,
                                    PricingTierId = s.PricingTierId,
                                    ParentStoreId = s.ParentStoreId,
                                    StoreType = st.Name,
                                    PricingTier = pt.Name,
                                    IsActive = s.IsActive,
                                };
                    // Apply server-side filters where possible
                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                        if (criteria.ContactPerson.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.ContactPerson) && x.ContactPerson.ToUpper().Contains(criteria.ContactPerson.ToUpper()));

                        if (criteria.PhoneNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToUpper().Contains(criteria.PhoneNumber.ToUpper()));

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }
                    // Switch to in-memory for complex string filters
                    var efStores = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efStores.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efStores = efStores.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Store>>(JsonSerializer.Serialize(efStores.ToList()));
                }
            }
            catch
            {
                throw;
            }

            return mLister;

        }

        public Domain.Store Upsert(Domain.Store mStore)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStore = context.Stores.FirstOrDefault(x => x.Id == mStore.Id);

                    if (efStore == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mStore.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efStore = new Persistence.Store
                        {
                            AuditId = audit.Id
                        };

                        context.Stores.Add(efStore);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efStore.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mStore.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    efStore.Name = mStore.Name;
                    efStore.ContactPerson = mStore.ContactPerson;
                    efStore.PhoneNumber = mStore.PhoneNumber;
                    efStore.AlternativePhoneNumber = mStore.AlternativePhoneNumber;
                    efStore.AddressId = mStore.AddressId;
                    efStore.StoreTypeId = mStore.StoreTypeId;
                    efStore.PricingTierId = mStore.PricingTierId;
                    efStore.ParentStoreId = mStore.ParentStoreId;
                    efStore.IsActive = mStore.IsActive;

                    context.SaveChanges();

                    mStore.Id = efStore.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mStore;
        }

        public Domain.Store Get(int id)
        {
            var mStore = new Domain.Store();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStore = (from s in context.Stores
                                   where s.Id == id
                                   select new
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       ContactPerson = s.ContactPerson,
                                       PhoneNumber = s.PhoneNumber,
                                       AlternativePhoneNumber = s.AlternativePhoneNumber,
                                       AddressId = s.AddressId,
                                       StoreTypeId = s.StoreTypeId,
                                       PricingTierId = s.PricingTierId,
                                       ParentStoreId = s.ParentStoreId,
                                       IsActive = s.IsActive,
                                   }).FirstOrDefault();

                    if (efStore != null)
                    {
                        mStore = JsonSerializer.Deserialize<Domain.Store>(JsonSerializer.Serialize(efStore));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mStore;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStore = context.Stores.FirstOrDefault(x => x.Id == id);
                    if (efStore != null)
                    {
                        var audit = context.AuditTrails.Find(efStore.AuditId);
                        if (audit != null)
                        {
                            audit.DeletedBy = loginUserId;
                            audit.DeletedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAddress(int id, int addressId, int? loginUserId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStore = context.Stores.FirstOrDefault(x => x.Id == id);
                    if (efStore != null)
                    {
                        efStore.AddressId = addressId;
                        var audit = context.AuditTrails.Find(efStore.AuditId);
                        if (audit != null)
                        {
                            audit.DeletedBy = loginUserId;
                            audit.DeletedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.Store> GetAllParentStore()
        {
            var mStore = new List<Domain.Store>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStore = (from s in context.Stores
                                   join at in context.AuditTrails on s.AuditId equals at.Id
                                   where at.DeletedDate == null && at.DeletedBy == null && s.IsActive && s.StoreTypeId == 1
                                   select new
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       IsActive = s.IsActive,
                                   }).AsEnumerable();

                    mStore = JsonSerializer.Deserialize<List<Domain.Store>>(JsonSerializer.Serialize(efStore.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mStore;
        }

        public List<Domain.Store> GetActiveStore()
        {
            var mStore = new List<Domain.Store>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStore = from s in context.Stores
                                  join at in context.AuditTrails on s.AuditId equals at.Id
                                  where at.DeletedBy == null && at.DeletedDate == null
                                  select new
                                  {
                                      Id = s.Id,
                                      Name = s.Name + " (" + s.ContactPerson + ")",
                                      AuditId = s.AuditId
                                  };

                    mStore = JsonSerializer.Deserialize<List<Domain.Store>>(JsonSerializer.Serialize(efStore.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mStore;
        }

    }
}
