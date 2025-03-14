using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
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