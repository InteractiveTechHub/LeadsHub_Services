
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ILeadBrokerRepository
    {
        /// <summary>
        /// Find next lead according to the distribuition rule
        /// </summary>
        /// <param name="functionQuery"></param>
        /// <returns>Consultant to be assigned in the lead</returns>
        Task<SimpleResponse<Consultant>> FetchNextConsultant(long companyId);

        /// <summary>
        /// Register the lead
        /// </summary>
        /// <param name="lead">Lead to be registered</param>
        /// <returns></returns>
        Task<SimpleResponse<Lead>> RegisterLeadAsync(Lead lead);

        /// <summary>
        /// Fetch lead
        /// </summary>
        /// <param name="contact">The data to find the lead</param>
        /// <returns>Return Existing Lead</returns>
        Task<SimpleResponse<Lead?>> FetchLeadByContactAsync(Contact customer);

        Task UpdateConsultantsAsync(Consultant consultant);
    }
}
