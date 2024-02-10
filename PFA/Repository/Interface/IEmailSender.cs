namespace PFA.Repository.Interface
{
    public interface IEmailSender
    {
        Task<bool> EmailSendAsync(string email, string v1, string v2);
        Task<bool> EmailSenderAsync(string email,string Subject,string message);
    }

}
