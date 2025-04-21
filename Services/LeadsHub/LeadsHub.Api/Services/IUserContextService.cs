using LeadsHub.Core.Identity;

namespace LeadsHub.Api.Services
{
    public interface IUserContextService
    {
        Task<UserContext> GetUserContextAsync();
    }
}
