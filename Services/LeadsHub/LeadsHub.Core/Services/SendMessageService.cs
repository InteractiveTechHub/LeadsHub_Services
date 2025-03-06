
using AdaptiveKitCore.Responses;
using CrossCutting.Models;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Utility;
using System.Text.Json;


namespace LeadsHub.Core.Services
{
    public sealed class SendMessageService(IHttpClientFactory httpClientFactory) : BaseHttpService(httpClientFactory), ISendMessageService
    {
        public async Task<BaseResponse> SendMessageToWhatsApp(TransferLead transfer)
        {
            string messageJson = JsonSerializer.Serialize(transfer);
            string url = $"{SD.WhatAppApiUrl}/sendmessage";

            BaseResponse response = await SendMessageAsync(url, messageJson);

            return response;
        }
    }
}
