namespace MedicalAppointmentsSystem.Services.MailService
{
    public interface IEmailService
    {
        bool SendMail(MailData Mail_Data);
    }
}
