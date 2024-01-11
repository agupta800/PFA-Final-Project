namespace PFA.ViewModel.Email
{
    public class GetEmailSetting
    {
        public string SecretKey { get; set; } = default!;
        public string From { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int Port { get; set; } = default!;
        public bool EnableSSl { get; set; } = default!;
    }
}
