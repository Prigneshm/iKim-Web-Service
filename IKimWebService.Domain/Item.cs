using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class Item : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
        public int HSNCode { get; set; }
        public decimal GST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
        public List<ItemPrice> Prices { get; set; } = new List<ItemPrice>();
    }
}
