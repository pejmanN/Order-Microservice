using Framework.Core;

namespace Framework.Domain.EventOutbox
{
    public interface IWorkerOutboxRepository : IRepository
    {
        Task<List<DomainEventItem>> GetReadyToSendMessages();
        Task SaveAsync();
    }
}
