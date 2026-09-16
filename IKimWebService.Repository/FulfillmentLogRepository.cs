using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKimWebService.Repository
{
    public class FulfillmentLogRepository : IFulfillmentLogRepository
    {
        public Domain.FulfillmentLogLister GetAll(Domain.FulfillmentLogLister mLister)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var query = from f in context.FulfillmentLogs
                                join at in context.AuditTrails on f.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = f.Id,
                                    OrderLineId = f.OrderLineId,
                                    QuantityFulfilled = f.QuantityFulfilled,
                                    Status = f.Status,
                                    Date = f.Date,
                                    Notes = f.Notes,
                                };

                    // Switch to in-memory for complex string filters
                    var efFulfillmentLogs = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efFulfillmentLogs.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efFulfillmentLogs = efFulfillmentLogs.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.FulfillmentLog>>(JsonSerializer.Serialize(efFulfillmentLogs.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.FulfillmentLog Upsert(Domain.FulfillmentLog mFulfillmentLog)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efFulfillmentLog = context.FulfillmentLogs.FirstOrDefault(x => x.Id == mFulfillmentLog.Id);
                    //var efFulfillmentLog = context.FulfillmentLogs.FirstOrDefault(x => x.OrderLineId == mFulfillmentLog.OrderLineId && x.Status == mFulfillmentLog.Status);
                    if (efFulfillmentLog == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mFulfillmentLog.CreatedBy,
                            CreatedDate = DateTime.Now
                        };
                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efFulfillmentLog = new Persistence.FulfillmentLog
                        {
                            AuditId = audit.Id
                        };
                        context.FulfillmentLogs.Add(efFulfillmentLog);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efFulfillmentLog.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mFulfillmentLog.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efFulfillmentLog.OrderLineId = mFulfillmentLog.OrderLineId;
                    efFulfillmentLog.QuantityFulfilled = mFulfillmentLog.QuantityFulfilled;
                    efFulfillmentLog.Status = mFulfillmentLog.Status;
                    efFulfillmentLog.Date = DateTime.Now; 
                    efFulfillmentLog.Notes = mFulfillmentLog.Notes;

                    context.SaveChanges();
                    mFulfillmentLog.Id = efFulfillmentLog.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mFulfillmentLog;
        }

        public Domain.FulfillmentLog Get(int id)
        {
            var mFulfillmentLog = new Domain.FulfillmentLog();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efFulfillmentLog = (from f in context.FulfillmentLogs
                                            where f.Id == id
                                            select new
                                            {
                                                Id = f.Id,
                                                OrderLineId = f.OrderLineId,
                                                QuantityFulfilled = f.QuantityFulfilled,
                                                Status = f.Status,
                                                Date = f.Date,
                                                Notes = f.Notes,
                                            }).FirstOrDefault();

                    if (efFulfillmentLog != null)
                    {
                        mFulfillmentLog = JsonSerializer.Deserialize<Domain.FulfillmentLog>(JsonSerializer.Serialize(efFulfillmentLog));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mFulfillmentLog;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efFulfillmentLog = context.FulfillmentLogs.FirstOrDefault(x => x.Id == id);
                    if (efFulfillmentLog != null)
                    {
                        var audit = context.AuditTrails.Find(efFulfillmentLog.AuditId);
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
