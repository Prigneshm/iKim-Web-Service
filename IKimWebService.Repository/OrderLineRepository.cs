using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace IKimWebService.Repository
{
    public class OrderLineRepository : IOrderLineRepository
    {

        public Domain.OrderLineLister GetAll(Domain.OrderLineLister mLister)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var query = from ol in context.OrderLines
                                join i in context.Items on ol.ItemId equals i.Id
                                join at in context.AuditTrails on ol.AuditId equals at.Id
                                join f in context.FulfillmentLogs on ol.Id equals f.OrderLineId into joinIFL
                                from f in joinIFL.DefaultIfEmpty()
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = ol.Id,
                                    OrderId = ol.OrderId,
                                    ItemId = ol.ItemId,
                                    Item = i.Name,
                                    QuantityRequested = ol.QuantityRequested,
                                    QuantityFulfilled = f != null ? f.QuantityFulfilled : 0,
                                    Status = f != null ? f.Status : "Pending"
                                };

                    // Switch to in-memory for complex string filters

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.OrderId > 0)
                            query = query.Where(x => x.OrderId == criteria.OrderId);
                    }
                    var efOrderLines = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efOrderLines.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efOrderLines = efOrderLines.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.OrderLine>>(JsonSerializer.Serialize(efOrderLines.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.OrderLine Upsert(Domain.OrderLine mOrderLine)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrderLine = context.OrderLines.FirstOrDefault(x => x.Id == mOrderLine.Id);
                    if (efOrderLine == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mOrderLine.CreatedBy,
                            CreatedDate = DateTime.Now
                        };
                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efOrderLine = new Persistence.OrderLine
                        {
                            AuditId = audit.Id
                        };
                        context.OrderLines.Add(efOrderLine);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efOrderLine.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mOrderLine.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efOrderLine.OrderId = mOrderLine.OrderId;
                    efOrderLine.ItemId = mOrderLine.ItemId;
                    efOrderLine.QuantityRequested = mOrderLine.QuantityRequested;

                    context.SaveChanges();
                    mOrderLine.Id = efOrderLine.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrderLine;
        }

        public Domain.OrderLine Get(int id)
        {
            var mOrderLine = new Domain.OrderLine();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrderLine = (from ol in context.OrderLines
                                       join i in context.Items on ol.ItemId equals i.Id
                                       where ol.Id == id
                                       select new
                                       {
                                           Id = ol.Id,
                                           OrderId = ol.OrderId,
                                           ItemId = ol.ItemId,
                                           Item = i.Name,
                                           QuantityRequested = ol.QuantityRequested,
                                       }).FirstOrDefault();

                    if (efOrderLine != null)
                    {
                        mOrderLine = JsonSerializer.Deserialize<Domain.OrderLine>(JsonSerializer.Serialize(efOrderLine));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrderLine;
        }

        public List<Domain.OrderLine> GetBy(int orderId)
        {
            var mOrderLines = new List<Domain.OrderLine>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrderLines = (from ol in context.OrderLines
                                        join at in context.AuditTrails on ol.AuditId equals at.Id
                                        where ol.OrderId == orderId
                                        select new
                                        {
                                            Id = ol.Id,
                                            ItemId = ol.ItemId
                                        }).AsEnumerable();

                    mOrderLines = JsonSerializer.Deserialize<List<Domain.OrderLine>>(JsonSerializer.Serialize(efOrderLines));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrderLines;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efOrderLine = context.OrderLines.FirstOrDefault(x => x.Id == id);
                    if (efOrderLine != null)
                    {
                        var audit = context.AuditTrails.Find(efOrderLine.AuditId);
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
