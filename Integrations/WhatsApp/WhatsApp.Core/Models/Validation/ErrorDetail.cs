
namespace WhatsApp.Core.Models.Validation
{
    public sealed class ErrorDetail
    {
        /// <summary>
        /// Message genarated in the facebookgraph/whatsapp api 
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Facebook/whatsapp api error type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Code of the error
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Subcode of error
        /// </summary>
        public int ErrorSubcode { get; set; }

        /// <summary>
        /// The trace id of the error
        /// </summary>
        public string FbTraceId { get; set; } = string.Empty;
    }
}
