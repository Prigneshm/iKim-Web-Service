using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class FulfillmentLogLister
    {
        public List<FulfillmentLog> List { get; set; } = new List<FulfillmentLog>();

        public FulfillmentLog SearchCriteria { get; set; } = new FulfillmentLog();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
