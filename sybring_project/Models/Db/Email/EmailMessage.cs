namespace sybring_project.Models.Db.Email
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddress = new EmailAddress();
            Subject = string.Empty;
            Content = string.Empty;
        }

        public List<EmailAddress> ToAddresses { get; set; }
        public EmailAddress FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
