
namespace WhatsApp.Core.Request
{
    public class MessageRequest
    {
        /// <summary>
        /// Api url
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Data serialized
        /// </summary>
        public string DataJson { get; set; } = string.Empty;

        /// <summary>
        /// Token to access api
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}
