using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Museu_DB_v02c.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
