using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Domain
{
    public class User : AuditTrail
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public int? StoreId { get; set; }
        public string Store { get; set; }
        public string UserType { get; set; }
        public string PlainTextPassword { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public int AuditId { get; set; }
    }
}
