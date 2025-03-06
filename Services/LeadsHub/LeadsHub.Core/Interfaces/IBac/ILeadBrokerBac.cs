
using CrossCutting.Models;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ILeadBrokerBac
    {
        Task ReceiveLeadsAsync(TransferLead leadMessage);
    }
}
