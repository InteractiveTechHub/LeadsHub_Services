
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using InteractiveLeads.Core.Enums;
using LeadsHub.Api.Services;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Security.Claims;

namespace LeadsHub.Core.Identity
{
    public sealed class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private UserContext? _cachedUserContext;
        private readonly IConsultantBac _consultantBac;

        public UserContextService(IHttpContextAccessor httpContextAccessor,
                                  IMemoryCache memoryCache,
                                  IConsultantBac consultantBac)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _consultantBac = consultantBac;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        private IEnumerable<string> Roles =>
                    _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?.Select(c => c.Value) ?? Enumerable.Empty<string>();

        public FilterRequest GetFilterByPermissions(UserContext userContext)
        {
            FilterRequest filterRequest = new();

            if (userContext.IsConsultant)
            {
                filterRequest.AddFilter(nameof(SalesPipeline.ConsultantId), FilterOperatorEnum.Equals, userContext.ConsultantId);
            }

            string companyIds = string.Join(",", userContext.CompanyIds);
            filterRequest.AddFilter(nameof(SalesPipeline.CompanyId), FilterOperatorEnum.In, companyIds);

            return filterRequest;
        }

        public async Task<UserContext> GetUserContextAsync()
        {
            if (_cachedUserContext != null)
                return _cachedUserContext;

            var cacheKey = $"user-context:{UserId}";

            if (_memoryCache.TryGetValue(cacheKey, out UserContext? userContext))
            {
                _cachedUserContext = userContext;
                return userContext!;
            }

            var response = await _consultantBac.FetchConsultantByUserIdAsync(UserId!);
            if (response.HasAnyErrorMessage || response.Model == null)
            {
                throw new InvalidOperationException("Unable to fetch user context.");
            }

            userContext = response.Model;
            userContext!.IdentityId = UserId!;            
            userContext.Roles = [.. Roles];
            
            userContext = GetRoles(userContext);

            userContext.FilterRequest = GetFilterByPermissions(userContext);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, userContext, cacheEntryOptions);
            _cachedUserContext = userContext;

            return userContext;
        }

        private UserContext GetRoles(UserContext userContext)
        {
            userContext.IsSysAdmin = User!.IsInRole(RolesEnum.SysAdmin.Name);
            userContext.IsSupport = User!.IsInRole(RolesEnum.Support.Name);
            userContext.IsOwner = User!.IsInRole(RolesEnum.Owner.Name);
            userContext.IsManager = User!.IsInRole(RolesEnum.Manager.Name);
            userContext.IsConsultant = User!.IsInRole(RolesEnum.Consultant.Name);

            return userContext;
        }
    }
}
