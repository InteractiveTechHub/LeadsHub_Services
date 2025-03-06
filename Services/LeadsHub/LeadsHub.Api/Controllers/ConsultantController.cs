using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using InteractiveLeads.Core.Enums;
using LeadsHub.Core.Identity.Models;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class ConsultantController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConsultantBac _consultantBac;

        public ConsultantController(IConsultantBac consultantBac, UserManager<ApplicationUser> userManager)
        {
            _consultantBac = consultantBac;
            _userManager = userManager;
        }

        [HttpPost("fetchall")]
        public async Task<IActionResult> FetchAllConsultantsAsync(FilterRequest filterRequest)
        {
            //filterRequest = BuildUserPermitionFilterRequestAsync(filterRequest);

            ConsultantResponse response = await _consultantBac.FetchConsultantsByRequestAsync(filterRequest);
            if(response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }            

            return Ok(response);
        }

        private FilterRequest BuildUserPermitionFilterRequestAsync(FilterRequest filterRequest)
        {
            if (!User.IsInRole(RolesEnum.SysAdmin.Name))
            {
                filterRequest.AddFilter(nameof(IdentityRole.Name), FilterOperatorEnum.NotEquals, RolesEnum.SysAdmin.Name, "r");
            }

            if (!User.IsInRole(RolesEnum.Support.Name))
            {
                filterRequest.AddFilter(nameof(IdentityRole.Name), FilterOperatorEnum.NotEquals, RolesEnum.Support.Name, "r");
            }

            if (!User.IsInRole(RolesEnum.Owner.Name))
            {
                filterRequest.AddFilter(nameof(IdentityRole.Name), FilterOperatorEnum.NotEquals, RolesEnum.Owner.Name, "r");
            }

            if (!User.IsInRole(RolesEnum.Manager.Name))
            {
                filterRequest.AddFilter(nameof(IdentityRole.Name), FilterOperatorEnum.NotEquals, RolesEnum.Manager.Name, "r");
            }            

            return filterRequest;
        }
    }
}
