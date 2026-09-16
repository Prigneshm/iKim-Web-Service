using IKimWebService.Infrastructure.IService;
using IKimWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace IKimWebService.Repository
{
    public class StockItemRepository : IStockItemRepository
    {
        public Domain.StockItemLister GetAll(Domain.StockItemLister mLister)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var query = from sbi in context.StockItems
                                join i in context.Items on sbi.ItemId equals i.Id
                                join um in context.UnitOfMeasures on i.UnitOfMeasureId equals um.Id
                                join at in context.AuditTrails on sbi.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                orderby sbi.Id descending
                                select new Domain.StockItem
                                {
                                    Id = sbi.Id,
                                    StockId = sbi.StockId,
                                    ItemId = sbi.ItemId,
                                    Quantity = sbi.Quantity,
                                    QuantityWithUnit = sbi.Quantity + " " + um.Name,
                                    Item = i.Name
                                };

                    // Filter
                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.StockId > 0)
                            query = query.Where(x => x.StockId == criteria.StockId);

                        if (criteria.Item.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Item) && x.Item.ToUpper().Contains(criteria.Item.ToUpper()));

                    }

                    var efStockItem = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efStockItem.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                        ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                        : 1;

                    efStockItem = efStockItem.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.StockItem>>(JsonSerializer.Serialize(efStockItem.ToList()));
                }
            }
            catch
            {
                throw;
            }

            return mLister;
        }

        public Domain.StockItem Upsert(Domain.StockItem mStockItem)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStockItem = context.StockItems.FirstOrDefault(x => x.Id == mStockItem.Id);

                    if (efStockItem == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mStockItem.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efStockItem = new Persistence.StockItem
                        {
                            AuditId = audit.Id
                        };

                        context.StockItems.Add(efStockItem);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efStockItem.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mStockItem.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    efStockItem.StockId = mStockItem.StockId;
                    efStockItem.ItemId = mStockItem.ItemId;
                    efStockItem.Quantity = mStockItem.Quantity;

                    context.SaveChanges();

                    mStockItem.Id = efStockItem.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mStockItem;
        }

        public Domain.StockItem Get(int id)
        {
            var mStockItem = new Domain.StockItem();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStockItem = (from s in context.StockItems
                                        where s.Id == id
                                        select new
                                        {
                                            Id = s.Id,
                                            StockId = s.StockId,
                                            ItemId = s.ItemId,
                                            Quantity = s.Quantity,
                                        }).FirstOrDefault();

                    if (efStockItem != null)
                    {
                        mStockItem = JsonSerializer.Deserialize<Domain.StockItem>(JsonSerializer.Serialize(efStockItem));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mStockItem;
        }

        public List<Domain.StockItem> GetBy(int stockId)
        {
            var mStockItems = new List<Domain.StockItem>();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStockItems = (from si in context.StockItems
                                        join at in context.AuditTrails on si.AuditId equals at.Id
                                        where si.StockId == stockId
                                        select new
                                        {
                                            Id = si.Id,
                                            ItemId = si.ItemId
                                        }).AsEnumerable();

                    mStockItems = JsonSerializer.Deserialize<List<Domain.StockItem>>(JsonSerializer.Serialize(efStockItems));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mStockItems;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efStockItem = context.StockItems.FirstOrDefault(x => x.Id == id);
                    if (efStockItem != null)
                    {
                        var audit = context.AuditTrails.Find(efStockItem.AuditId);
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
