
using LeadsHub.Core.Models;
using Microsoft.AspNetCore.Http;

namespace LeadsHub.Core.Dtos
{
    public sealed class TimelineFormData
    {
        public Timeline Timeline { get; set; } = new();

        public IFormFile? FormFile { get; set; }
    }
}
