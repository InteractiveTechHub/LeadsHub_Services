using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ICustomerRepository
    {
        Task<SimpleResponse<long>> FetchContactIdAsync(Contact customer);

        Task<SimpleResponse<long>> RegisterContactAsync(Contact customer);
    }
}
