namespace WhatsApp.Core.Models.Error
{
    public sealed class ErrorData
    {
        /// <summary>
        /// The product messaging. It will always be whatsapp
        /// for cloud API responses
        /// </summary>
        public string MessageProduct { get; set; } = string.Empty;

        /// <summary>
        /// Error description an a description of the msot likely
        /// reason for the error.
        /// </summary>
        public string Details { get; set; } = string.Empty;
    }
}