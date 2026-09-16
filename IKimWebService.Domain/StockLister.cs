using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class StockLister
    {
        public List<Stock> List { get; set; } = new List<Stock>();

        public Stock SearchCriteria { get; set; } = new Stock();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
