namespace TurneroAPI.Settings
{
    public class GmailSettings
    {
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
    }
}
