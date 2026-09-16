using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class Store : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        public string Status { get; set; }
        public int AddressId { get; set; }
        public int StoreTypeId { get; set; }
        public string StoreType { get; set; }
        public int PricingTierId { get; set; }
        public int? ParentStoreId { get; set; }
        public string PricingTier { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
    }
}
