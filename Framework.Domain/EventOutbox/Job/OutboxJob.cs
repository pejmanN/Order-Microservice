using Microsoft.Extensions.Logging;
using Quartz;

namespace Framework.Domain.EventOutbox.Job
{
    [DisallowConcurrentExecution]
    public class OutboxJob : IJob
    {
        private readonly ILogger<OutboxJob> _logger;
        private readonly IWorkerOutboxRepository _repository;
        private readonly IOutboxMessagePublisher _messagePublisher;

        public OutboxJob(ILogger<OutboxJob> logger,
                         IWorkerOutboxRepository repository,
                         IOutboxMessagePublisher messagePublisher)
        {
            _logger = logger;
            _repository = repository;
            _messagePublisher = messagePublisher;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var readyToSendItems = await _repository.GetReadyToSendMessages();
            _logger.LogInformation($"Outbox count {readyToSendItems.Count}:date : {DateTime.Now.ToLongTimeString()}");

            foreach (var item in readyToSendItems)
            {
                var eventMessage = item.CreateMessage();
                await _messagePublisher.PublishAsync(eventMessage);
                item.ChangeToSentState();
            }

            if (readyToSendItems.Count != 0)
                await _repository.SaveAsync();

        }
    }
}
