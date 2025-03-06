using CrossCutting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsApp.Core.Interfaces.IBac;
using WhatsApp.Core.Response;
using WhatsApp.Core.Interfaces.IServices;
using WhatsApp.Core.PayLoads;

namespace Whatsapp.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppService _whatsAppService;
        private readonly IWhatsappSendMessageBac _whatsappSendMessageBac;

        public WhatsAppController(IWhatsAppService whatsAppService, IWhatsappSendMessageBac whatsappSendMessageBac)
        {
            _whatsAppService = whatsAppService;
            _whatsappSendMessageBac = whatsappSendMessageBac;
        }

        [AllowAnonymous]
        [HttpGet("webhook")]
        public IActionResult WebhookAsync(
            [FromQuery(Name = "hub.mode")] string mode, 
            [FromQuery(Name = "hub.challenge")] string challenge, 
            [FromQuery(Name = "hub.verify_token")] string verifyToken)
        {
            //string? mode = HttpContext.Request.Query["hub.mode"];
            //string? challenge = HttpContext.Request.Query["hub.challenge"];
            //string? verifyToken = HttpContext.Request.Query["hub.verify_token"];

            //TODO: verify if the token is the same of settled in facebook
            
            if (verifyToken.Equals("whatsapp"))
            {
                int challengeConverted = Convert.ToInt32(challenge);

                return Ok(challengeConverted);
            }          
  
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> WebhookAsync(WhatsappPayLoad whatsappPayLoad)
        {
            await _whatsappSendMessageBac.ReceiveMessageFromWhatsappAsync(whatsappPayLoad);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessageAsync(TransferLead request)
        {
            JsonResponse response = await _whatsAppService.SendMessageToWhatsappAsync(request);

            return Ok(response);
        }
    }  
}
