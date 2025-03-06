
namespace CrossCutting.Models
{
    public class TransferLead
    {
        #region lead info
        public long CompanyId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Channel { get; set; } = string.Empty;

        public long IntegrationId { get; set; }
        #endregion

        #region Message info
        public string MessageId { get; set; } = string.Empty;

        public DateTimeOffset MessageDate { get; set; }

        public int MessageType { get; set; }
        #endregion

        #region Message content

        public string MessageBody { get; set; } = string.Empty;

        public string ReactionEmoji { get; set; } = string.Empty;

        public string MessageReactionId { get; set; } = string.Empty;

        public string MimeType { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string TemplateName { get; set; } = string.Empty;
        #endregion

        public void ConvertsTimeUnixToUtcDateTime(string timesStamp)
        {
            long timestampUnix = Convert.ToInt64(timesStamp);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestampUnix);

            MessageDate = dateTimeOffset.UtcDateTime;
        }
    }
}
