
using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Models
{
    public class Timeline : BaseModel
    {
        /// <summary>
        /// Company representant that the lead is talking with
        /// </summary>
        public long? ConsultantId { get; set; }

        /// <summary>
        /// The lead Id
        /// </summary>
        public long LeadId { get; set; }

        /// <summary>
        /// The message Id of the external system
        /// </summary>
        public string MessageId { get; set; } = string.Empty;

        /// <summary>
        /// The message date of the external system
        /// or sent by our system
        /// </summary>
        public DateTimeOffset MessageDate { get; set; } = new();

        /// <summary>
        /// The message file Id
        /// </summary>
        public long? MessageFileId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? MessageReactionId { get; set; }

        /// <summary>
        /// The message text Id
        /// </summary>
        public long? MessageTextId { get; set; }

        /// <summary>
        /// The template Id
        /// </summary>
        public long? TemplateId { get; set; }

        /// <summary>
        /// The date time that the message was read.
        /// </summary>
        public DateTimeOffset? ReadAt { get; set; }

        /// <summary>
        /// Who sent the message (Lead or Consultant)
        /// </summary>
        public MessageSender Sender { get; set; }

        /// <summary>
        /// The status of the message (Ex.: Pedding, Sent, Read, etc)
        /// </summary>
        public MessageStatus Status { get; set; }

        /// <summary>
        /// The message type (Ex.: Text, Image, Video, template, etc)
        /// </summary>
        public MessageType Type { get; set; }

        public MessageText? Message { get; set; }

        public MessageFile? MessageFile { get; set; }

        public MessageReaction? MessageReaction { get; set; }

        public bool IsFile => Type.Equals(MessageType.Image) 
            || Type.Equals(MessageType.Audio) 
            || Type.Equals(MessageType.Video) 
            || Type.Equals(MessageType.Document) 
            || Type.Equals(MessageType.Sticker);

        public void ConvertsTimeUnixToUtcDateTime(string timesStamp)
        {
            long timestampUnix = Convert.ToInt64(timesStamp);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestampUnix);

            MessageDate = dateTimeOffset.UtcDateTime;
        }
    }
}
