
using Newtonsoft.Json;

namespace LeadsHub.Core.Payloads.Whatsapp.Response
{
    public sealed class WhatsappResponse
    {
        [JsonProperty("messaging_product")]
        public string MessageProduct { get; set; } = string.Empty;

        public IEnumerable<ResponseContact> Contacts { get; set; } = [];

        public IEnumerable<ResponseMessage> Messages { get; set; } = [];
    }

    public sealed class ResponseContact
    {
        public string Input { get; set; } = string.Empty;

        [JsonProperty("wa_id")]
        public string WaId { get; set; } = string.Empty;
    }

    public sealed class ResponseMessage
    {
        public string Id { get; set; } = string.Empty;
    }
}
