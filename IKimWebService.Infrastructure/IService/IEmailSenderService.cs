using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebService.Infrastructure.IService
{
    public interface IEmailSenderService
    {
        //Task SendEmailAsync(string htmlBody, string toEmail, string name, int maxRetries = 3);
        Task SendEmailAsync(string subject, string htmlBody, string toEmail, int maxRetries = 3);
    }
}
