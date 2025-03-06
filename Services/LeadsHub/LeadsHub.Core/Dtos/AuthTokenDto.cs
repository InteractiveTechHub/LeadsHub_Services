

using LeadsHub.Core.Models;

namespace LeadsHub.Core.Dtos
{
    public sealed class AuthTokenDto
    {
        public string Token { get; set; } = string.Empty;

        public int AttemptsRemaining { get; set; } = 0;
    }
}
