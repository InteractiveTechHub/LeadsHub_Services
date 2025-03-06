
using AdaptiveKitCore.Enums;

namespace AdaptiveKitCore.Model
{
    /// <summary>
    /// Encapsulates a message and all it's metadata or information.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// Default empty constructor
        /// </summary>
        public Message()
        {
        }

        public Message(MessageTypeEnum messageType, string messageText = "", string messageCode = "")
        {
            MessageType = messageType;
            MessageText = messageText;
            MessageCode = messageCode;
        }

        public MessageTypeEnum MessageType { get; private set; } = MessageTypeEnum.None;

        public string MessageText { get; private set; } = string.Empty;

        public string MessageCode { get; private set; } = string.Empty;
    }
}
