using Framework.Domain.EventOutbox;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Outbox.Infra.Persistence
{
    public class WorkerOutboxRepository : IWorkerOutboxRepository
    {
        private readonly OrderOutboxDbContext _orderOutboxDbContext;

        public WorkerOutboxRepository(OrderOutboxDbContext orderOutboxDbContext)
        {
            _orderOutboxDbContext = orderOutboxDbContext;
        }

        public async Task<List<DomainEventItem>> GetReadyToSendMessages()
        {
            return await _orderOutboxDbContext.DomainEventItems
                  .Where(x => x.PublishDateTime.Date >= DateTime.Now.Date && x.IsSent == false)
                  .Take(50)
                  .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _orderOutboxDbContext.SaveChangesAsync();
        }
    }
}
