namespace WhatsApp.Core.PayLoads
{
    public sealed class PayLoadChange
    {
        public string Field { get; set; } = string.Empty;

        public PayLoadValue Value { get; set; } = new();
    }
}
