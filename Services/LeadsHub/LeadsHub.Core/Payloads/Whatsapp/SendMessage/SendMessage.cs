namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
{
    public sealed class SendMessage
    {
        /// <summary>
        /// Id of the operation
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The message develivery status
        /// </summary>
        public string Message_Status { get; set; } = string.Empty;
    }
}