namespace WhatsApp.Core.PayLoads
{
    public sealed class PayloadEntry
    {
        public IEnumerable<PayLoadChange> Changes { get; set; } = [];

        public string Id { get; set; } = string.Empty;

    }
}