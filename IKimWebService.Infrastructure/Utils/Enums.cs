using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.Utils
{
    public static class Enums
    {
        public enum UserType
        {
            Super_Admin = 1,
            Admin = 2,
            User = 3,
        }

        public enum Status
        {
            Pending = 1,
            Partially_Fulfilled = 2,
            Fulfilled = 3,
            NextDay = 4,
        }

    }
}
