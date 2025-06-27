using MailKit.Security;
using MedicalAppointmentsSystem.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace MedicalAppointmentsSystem
{
    public class EmailSender : IEmailSender
    {
       
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}

