using AdaptiveKitCore.Responses;
using LeadsHub.Api.Services;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Identity.Models;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public sealed class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IConsultantBac _consultantBac;

        public AuthController(
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            JwtService jwtService,
            IConsultantBac consultantBac)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _consultantBac = consultantBac;
        }

        [Authorize(Roles = "SysAdmin, Admin, Owner, Manager")]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUserAsync([FromBody] ConsultantDto model)
        {
            ModelResponse response = new();

            // TODO: Password Should be randomic.
            // TODO: Password will be randomic when it will be able to send email.
            string password = "#NewUser-2024";

            ApplicationUser user = new()
            {  
                Email = model.Email,
                Enabled = model.Enabled,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    response.AddErrorMessage(error.Code);           
                }

                if (response.HasErrorMessage) 
                    return BadRequest(response);

                response.AddErrorMessage("Não foi possível criar autenticação para o usuário");

                return BadRequest(response);
            }

            result = await _userManager.AddToRoleAsync(user, model.Roles);
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (IdentityError error in result.Errors)
                {
                    response.AddErrorMessage(error.Code);
                }

                if (response.HasAnyErrorMessage)
                    return BadRequest(response);

                response.AddErrorMessage("Erro ao tentar atribuir permissao ao usuário.");

                return BadRequest(response);
            }

            model.IdentityId = user.Id;

            ModelResponse rep = await CreatesConsultantAsync(model);
            if (rep.HasAnyErrorMessage)
            {
                await _userManager.DeleteAsync(user);

                response.Messages.AddRange(rep.Messages);

                return BadRequest(response);
            }

            //TODO: SEND EMAIL TO NEW USER.

            response.AddSuccessMessage("UserCreated");

            return Ok(response);
        }

        /// <summary>
        /// The user can sign in using email or username
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            TokenResponse response = new();

            ApplicationUser? user = await GetUserByNameOrEmailAsync(model.UserName);
            if (user is null)
            {
                GenerateErrorResponse(response, "AuthenticationInvalid", "001");
                return Unauthorized(response);
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                GenerateErrorResponse(response, "AuthenticationBlocked", "002");
                return Unauthorized(response);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    GenerateErrorResponse(response, "AuthenticationBlocked", "003");
                    return Unauthorized(response);
                }                    

                int attemptsRemaining = await GetRemainingAttemptsAsync(user);

                response.AddErrorMessage("AuthenticationInvalid", "004");
                response.Model.AttemptsRemaining = attemptsRemaining;

                return Unauthorized(response);
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            string token = _jwtService.GenerateToken(user, userRoles);
            response.Model.Token = token;

            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] ConsultantDto model)
        {
            ModelResponse response = new();

            ApplicationUser? user = await GetUserByNameOrEmailAsync(model.Email!);
            if (user is null)
            {
                response.AddErrorMessage("AuthenticationInvalid");
                return Unauthorized(response);
            }

            user.Email = model.Email;
            user.Enabled = model.Enabled;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;           

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) 
            {
                foreach (IdentityError error in result.Errors) 
                {
                    response.AddErrorMessage(error.Code);
                }
                
                return BadRequest(response);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    response.AddErrorMessage(error.Code);
                }

                if (response.HasErrorMessage)
                    return BadRequest(response);

                response.AddErrorMessage("ErrorRolesUpdate");
                return BadRequest(response);
            }

            if (!string.IsNullOrWhiteSpace(model.Roles))
            {
                result = await _userManager.AddToRoleAsync(user, model.Roles);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        response.AddErrorMessage(error.Code);
                    }

                    response.AddErrorMessage("ErrorRolesUpdate");
                    return BadRequest(response);
                }
            }

            model.IdentityId = user.Id;

            ModelResponse consultantResponse = await UpdateConsultantAsync(model);
            if (consultantResponse.HasAnyErrorMessage)
            {
                return BadRequest(consultantResponse);
            }

            response.AddSuccessMessage("UserUpdated");

            return Ok(response);
        }

        private async Task<ModelResponse> CreatesConsultantAsync(ConsultantDto model)
        {
            Consultant consultant = new()
            {
                IdentityId = model.IdentityId,
                FullName = model.FullName,
                NickName = model.NickName,
                Companies = model.Companies,
                Enabled = model.Enabled,
            };

            ModelResponse response = await _consultantBac.CreatesConsultantAsync(consultant);

            return response;
        }

        private async Task<int> GetRemainingAttemptsAsync(ApplicationUser user)
        {
            int accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            int maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
            return maxAttempts - accessFailedCount;
        }


        private async Task<ApplicationUser?> GetUserByNameOrEmailAsync(string userNameOrEmail)
        {
            return await _userManager.FindByNameAsync(userNameOrEmail) ??
                   await _userManager.FindByEmailAsync(userNameOrEmail);
        }

        private TokenResponse GenerateErrorResponse(TokenResponse response, string errorMessage, string errorCode)
        {
            response.AddErrorMessage(errorMessage, errorCode);
            return response;
        }

        private async Task<ModelResponse> UpdateConsultantAsync(ConsultantDto model)
        {
            Consultant consultant = new()
            {
                Id = model.Id,
                IdentityId = model.IdentityId,
                FullName = model.FullName,
                NickName = model.NickName,
                Companies = model.Companies,
                Enabled = model.Enabled,
            };

            ModelResponse response = await _consultantBac.UpdateConsultantAsync(consultant);

            return response;
        }
    }
}
