namespace WhatsApp.Core.PayLoads
{
    public sealed class PayLoadText
    {
        /// <summary>
        /// This is the message that user writes and send
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}