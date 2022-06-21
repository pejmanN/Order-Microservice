namespace Framework.Domain.EventOutbox
{
    public interface IOutboxMessagePublisher
    {
        Task PublishAsync(object @event);        
    }
}
