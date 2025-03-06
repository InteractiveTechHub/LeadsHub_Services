namespace WhatsApp.Core.PayLoads
{
    public sealed class PayLoadValue
    {
        public string Messaging_Product { get; set; } = string.Empty;

        public PayLoadMetadata Metadata { get; set; } = new();

        public IEnumerable<PayLoadContact> Contacts { get; set; } = [];

        public IEnumerable<PayLoadMessage> Messages { get; set; } = [];
    }
}