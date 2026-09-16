using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class UserLister
    {
        public List<User> List { get; set; } = new List<User>();

        public User SearchCriteria { get; set; } = new User();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
