using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        void Send(string to, string subject, string html, string from = null);
    }
}
