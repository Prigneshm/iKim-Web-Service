using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class ItemLister
    {
        public List<Item> List { get; set; } = new List<Item>();

        public Item SearchCriteria { get; set; } = new Item();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
