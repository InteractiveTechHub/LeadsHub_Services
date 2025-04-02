

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface IActiveChatManager
    {
        void AddLead(long leadId, DateTimeOffset lastMessageTime);
        void RemoveLead(long leadId);
        bool CanSendFreeMessage(long leadId);
    }
}
