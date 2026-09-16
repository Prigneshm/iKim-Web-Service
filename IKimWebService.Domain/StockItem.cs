using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class StockItem : AuditTrail
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public string Stock { get; set; }
        public string Item { get; set; }
        public string QuantityWithUnit { get; set; }
        public int Quantity { get; set; }
        public int AuditId { get; set; }
    }
}
