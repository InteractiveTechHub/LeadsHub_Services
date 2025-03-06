
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly ILeadBrokerRepository _leadRepository;

        public DistributionService(ILeadBrokerRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        /// <summary>
        /// Automatically finds the next consultant for assigning to the lead
        /// </summary>
        /// <param name="companyId">CompanyId to find the consuntants of the company</param>
        /// <returns>The consultant to be assigned</returns>
        public async Task<Consultant> DistributeLeadsAsync(long companyId) // TODO: Move it to class to share it.
        {
            // Should verify the company configuration
            /*
             * Queue (Sequential) distribution
             * Less lead distribuition
             * Close lead distribuition (it is not trigged here)
             * Manual distribuition (Manager should choose who will attend the lead)
             */

            //string sequencialRuleQuery = $"SELECT * FROM sequencial_next_consultant({companyId})";

            SimpleResponse<Consultant> consultantResponse = await _leadRepository.FetchNextConsultant(companyId);
            if (consultantResponse.HasErrorMessage || consultantResponse.HasExceptionMessage)
            {
                // Register a log
            }

            return consultantResponse.Model;
        }
    }
}
