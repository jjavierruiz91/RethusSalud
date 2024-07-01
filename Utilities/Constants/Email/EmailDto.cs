namespace rethus_backend.Utilities.Constants.Email.EmailDto
{
    public class SendEmailDto
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public string BodyPath { get; set; }
    }

    public class PlaceHoldersTemplate
    {
        public required string LinkRestorePassword { get; set; }
    }
}
