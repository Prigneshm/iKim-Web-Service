using IKimWebService.Infrastructure.Helper;
using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text.Json;

namespace IKimWebService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public Domain.OrderLister GetAll(Domain.OrderLister mLister)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var query = from o in context.Orders
                                join at in context.AuditTrails on o.AuditId equals at.Id
                                join s in context.Stores on o.StoreId equals s.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = o.Id,
                                    Date = o.Date,
                                    StoreId = o.StoreId,
                                    Store = s.Name,
                                    Status = o.Status,
                                };

                    // Switch to in-memory for complex string filters
                    var efOrders = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efOrders.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efOrders = efOrders.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Order>>(JsonSerializer.Serialize(efOrders.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Order Upsert(Domain.Order mOrder)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrder = context.Orders.FirstOrDefault(x => x.Id == mOrder.Id);
                    if (efOrder == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mOrder.CreatedBy,
                            CreatedDate = DateTime.Now
                        };
                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efOrder = new Persistence.Order
                        {
                            AuditId = audit.Id,
                            Status = EnumsHelper.GetEnumString(Enums.Status.Pending)
                        };
                        context.Orders.Add(efOrder);
                    }
                    else
                    {
                        efOrder.Status = mOrder.Status;
                        var audit = context.AuditTrails.Find(efOrder.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mOrder.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efOrder.Date = DateTime.Now;
                    efOrder.StoreId = mOrder.StoreId;

                    context.SaveChanges();
                    mOrder.Id = efOrder.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrder;
        }

        public Domain.Order Get(int id)
        {
            var mOrder = new Domain.Order();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrder = (from o in context.Orders
                                  where o.Id == id
                                  select new
                                  {
                                      Id = o.Id,
                                      Date = o.Date,
                                      StoreId = o.StoreId,
                                      Status = o.Status,
                                  }).FirstOrDefault();

                    if (efOrder != null)
                    {
                        mOrder = JsonSerializer.Deserialize<Domain.Order>(JsonSerializer.Serialize(efOrder));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrder;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrder = context.Orders.FirstOrDefault(x => x.Id == id);
                    if (efOrder != null)
                    {
                        var audit = context.AuditTrails.Find(efOrder.AuditId);
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
    }
}
