using Framework.Core.Events;

namespace Shared.Contracts
{
    public class CustomerEnabled : DomainEvent
    {
        public CustomerEnabled(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; private set; }
    }

    public class CustomerDisabled : DomainEvent
    {
        public CustomerDisabled(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; private set; }
    }
}
