using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class Stock : AuditTrail
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Store { get; set; }
        public DateTime EntryDate { get; set; }
        public int AuditId { get; set; }
    }
}
