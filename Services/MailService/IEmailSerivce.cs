namespace MedicalAppointmentsSystem.Services.MailService
{
    public interface IEmailSerivce
    {
        bool SendMail(MailData Mail_Data);
    }
}
