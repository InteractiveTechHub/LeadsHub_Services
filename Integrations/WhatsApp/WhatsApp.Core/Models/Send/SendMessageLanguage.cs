using System.Text.Json.Serialization;

namespace WhatsApp.Core.Models.Send
{
    public sealed class SendMessageLanguage
    {
        /// <summary>
        /// Language code
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = "pt_BR";
    }
}