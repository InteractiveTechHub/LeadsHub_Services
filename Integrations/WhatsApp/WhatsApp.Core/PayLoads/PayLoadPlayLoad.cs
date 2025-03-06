namespace WhatsApp.Core.PayLoads
{
    public sealed class WhatsappPayLoad
    {
        public List<PayloadEntry> Entry { get; set; } = new();

        public string Object { get; set; } = string.Empty;
    }
}
