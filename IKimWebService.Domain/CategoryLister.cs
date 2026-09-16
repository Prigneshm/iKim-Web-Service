using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class CategoryLister
    {
        public List<Category> List { get; set; } = new List<Category>();

        public Category SearchCriteria { get; set; } = new Category();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
