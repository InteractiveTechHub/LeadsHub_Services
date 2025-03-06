using WhatsApp.Core.PayLoads.Contact;

namespace WhatsApp.Core.PayLoads
{
    public sealed class PayLoadContactToReceive
    {
        public IEnumerable<PayLoadAddress> Addresses { get; set; } = [];

        public string BirthDay { get; set; } = string.Empty;

        public IEnumerable<PayLoadEmail> Emails { get; set; } = [];

        public PayLoadName Name { get; set; } = new();

        public PayLoadOrg Org { get; set; } = new();

        public IEnumerable<PayLoadPhone> Phones { get; set; } = [];

        public IEnumerable<PayLoadUrl> Urls { get; set; } = [];
    }
}
