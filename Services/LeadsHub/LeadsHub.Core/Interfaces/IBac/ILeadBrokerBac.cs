
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ILeadBrokerBac
    {
        Task ReceiveLeadsAsync(Lead lead);
    }
}
