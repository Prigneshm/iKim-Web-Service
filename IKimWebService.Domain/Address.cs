using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class Address : AuditTrail
    {
        public int Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int AuditId { get; set; }
        //public int CreatedBy { get; set; }
        //public int? LastModifiedBy { get; set; }
        public int StoreId { get; set; }
    }
}
