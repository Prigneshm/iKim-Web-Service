using IKimWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKimWebService.Repository
{
    public class ItemPriceRepository : IItemPriceRepository
    {
        public Domain.ItemPrice Upsert(Domain.ItemPrice mItemPrice)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efItemPrice = context.ItemPrices.FirstOrDefault(x => x.ItemId == mItemPrice.ItemId && x.PricingTierId == mItemPrice.PricingTierId);

                    if (efItemPrice == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mItemPrice.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efItemPrice = new Persistence.ItemPrice
                        {
                            AuditId = audit.Id
                        };

                        context.ItemPrices.Add(efItemPrice);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efItemPrice.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mItemPrice.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    efItemPrice.ItemId = mItemPrice.ItemId;
                    efItemPrice.PricingTierId = mItemPrice.PricingTierId;
                    efItemPrice.Price = mItemPrice.Price;

                    context.SaveChanges();

                    mItemPrice.Id = efItemPrice.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mItemPrice;
        }

        public Domain.ItemPrice Get(int id)
        {
            var mItemPrice = new Domain.ItemPrice();
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var efItemPrice = (from ip in context.ItemPrices
                                       where ip.Id == id
                                       select new
                                       {
                                           Id = ip.Id,
                                           ItemId = ip.ItemId,
                                           PricingTierId = ip.PricingTierId,
                                           Price = ip.Price,
                                       }).FirstOrDefault();

                    if (efItemPrice != null)
                    {
                        mItemPrice = JsonSerializer.Deserialize<Domain.ItemPrice>(JsonSerializer.Serialize(efItemPrice));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mItemPrice;
        }


        public List<Domain.ItemPrice> GetByItemId(int itemId)
        {
            try
            {
                using (var context = new Persistence.IKimDBEntities())
                {
                    var prices = (from ip in context.ItemPrices
                                  join pt in context.PricingTiers on ip.PricingTierId equals pt.Id
                                  where ip.ItemId == itemId
                                  select new Domain.ItemPrice
                                  {
                                      Id = ip.Id,
                                      ItemId = ip.ItemId,
                                      PricingTierId = ip.PricingTierId,
                                      Price = ip.Price,
                                      PricingTierName = pt.Name
                                  }).ToList();

                    return prices;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
