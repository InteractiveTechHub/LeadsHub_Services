
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class WhatsAppController : BaseController
    {
        private readonly IWhatsAppBac _whatsappBac;
        public WhatsAppController(IWhatsAppBac whatsAppBac)
        {
            _whatsappBac = whatsAppBac;
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

            int challengeConverted = Convert.ToInt32(challenge);

            /*if (verifyToken.Equals("tech_hub_leads"))
            {
                int challengeConverted = Convert.ToInt32(challenge);

                return Ok(challengeConverted);
            }*/

            return Ok(challengeConverted);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> WebhookAsync(WhatsAppPayLoad whatsappPayLoad)
        {
            await _whatsappBac.ReceiveMessageFromWhatsappAsync(whatsappPayLoad);

            return Ok();
        }
    }
}
