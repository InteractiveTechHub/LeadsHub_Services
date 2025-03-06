
namespace WhatsApp.Core.Models.Validation
{
    public sealed class FacebookGraphAPIError
    {
        public ErrorDetail Error { get; set; } = new();
    }
}
