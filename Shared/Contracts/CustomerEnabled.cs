using Framework.Core.Events;

namespace Shared.Contracts
{
    public class CustomerEnabled : DomainEvent
    {
        public CustomerEnabled(long customerId)
        {
            CustomerId = customerId;
        }

        public long CustomerId { get; private set; }
    }

    public class CustomerDisabled : DomainEvent
    {
        public CustomerDisabled(long customerId)
        {
            CustomerId = customerId;
        }

        public long CustomerId { get; private set; }
    }
}
