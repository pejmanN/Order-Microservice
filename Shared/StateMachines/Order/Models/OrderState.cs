using MassTransit;

namespace Shared.StateMachines.Order.Models
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public long OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
