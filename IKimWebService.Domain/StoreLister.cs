using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class StoreLister
    {
        public List<Store> List { get; set; } = new List<Store>();

        public Store SearchCriteria { get; set; } = new Store();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
