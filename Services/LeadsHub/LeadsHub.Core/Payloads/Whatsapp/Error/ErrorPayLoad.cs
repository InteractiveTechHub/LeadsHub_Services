namespace LeadsHub.Core.Payloads.Whatsapp.Error
{
    public sealed class ErrorPayLoad
    {
        public Error Error { get; set; } = new();
    }
}
