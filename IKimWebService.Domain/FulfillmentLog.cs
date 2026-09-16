using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class FulfillmentLog : AuditTrail
    {
        public int Id { get; set; }
        public int OrderLineId { get; set; }
        public int QuantityFulfilled { get; set; }
        public int QuantityRequested { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int AuditId { get; set; }
    }
}
