
using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads.Contact
{
    public class PayLoadUrl
    {
        /// <summary>
        /// Url of the website
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Website type
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
