using WhatsApp.Core.PayLoads.Message;

namespace WhatsApp.Core.PayLoads
{
    public sealed class PayLoadMessage
    {
        public string Id { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public PayLoadText Text { get; set; } = new();

        public PayloadReaction Reaction { get; set; } = new();

        public PayloadImage Image { get; set; } = new();

        public string TimeStamp { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public IEnumerable<PayLoadContactToReceive> Contacts { get; set; } = [];
    }
}