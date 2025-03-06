
using LeadsHub.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace LeadsHub.Core.Dtos
{
    public sealed class ApplicationUserDto
    {
        public ApplicationUserDto()
        {
        }

        public ApplicationUserDto(ApplicationUser applicationUser, IdentityRole? role)
        {
            BuildApplicationUser(applicationUser, role);
        }

        public string Id { get; private set; } = string.Empty;
        public int AccessFailedCount { get; private set; }
        public bool IsLockedOut { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public bool EmailConfirmed { get; private set; }
        public bool Enabled { get; set; }
        public string PhoneNumber { get; private set; } = string.Empty;        
        public string UserName { get; private set; } = string.Empty;
        public string RoleName { get; private set; } = string.Empty;

        private void BuildApplicationUser(ApplicationUser applicationUser, IdentityRole? role)
        {
            Id = applicationUser.Id;
            AccessFailedCount = applicationUser.AccessFailedCount;
            Email = applicationUser.Email ?? string.Empty;
            EmailConfirmed = applicationUser.EmailConfirmed;
            Enabled = applicationUser.Enabled;
            IsLockedOut = applicationUser.LockoutEnd.HasValue;
            RoleName = role?.Name ?? string.Empty;
            PhoneNumber = applicationUser.PhoneNumber ?? string.Empty;        
            UserName = applicationUser.UserName ?? string.Empty;
        }
    }
}
