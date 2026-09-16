using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class StockItemLister
    {
        public List<StockItem> List { get; set; } = new List<StockItem>();

        public StockItem SearchCriteria { get; set; } = new StockItem();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
