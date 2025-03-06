
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface ICompanyService
    {
        Task<SimpleResponse<Address>> GetAddressByCEP(string cep);

        Task<SimpleResponse<Company>> GetDataByCNPJAsync(string cnpj);
    }
}
