namespace PFA.Repository.Interface
{
    public interface IEmailSender
    {
        Task<bool> EmailSenderAsync(string email,string Subject,string message);
    }

}
