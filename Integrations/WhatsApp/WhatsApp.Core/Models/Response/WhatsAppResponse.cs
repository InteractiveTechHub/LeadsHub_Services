
using WhatsApp.Core.Models.Send;
using System.Text.Json.Serialization;

namespace WhatsApp.Core.Models.Response
{
    public sealed class WhatsappResponse
    {
        /// <summary>
        /// Product messging. This always will be 'whatsapp'
        /// </summary>
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; } = string.Empty;

        /// <summary>
        /// Contact that was delivered the request
        /// </summary>
        public List<SendMessageContact> Contacts { get; set; } = [];

        /// <summary>
        /// Message returned, message Id
        /// </summary>
        public List<ResponseMessage> Messages { get; set; } = []; 
    }
}
