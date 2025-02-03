using Framework.Core.Events;

namespace Shared.StateMachines.Order.Models
{
    public class ValidateCustomer
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class CustomerValidated : DomainEvent
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class AllocateInventory
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }
    public class InventorAllocated : DomainEvent
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }
    public class DeAllocateInventory
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class InventorDeAllocated : DomainEvent
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class DebitCustomer
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class CustomerDebited : DomainEvent
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class CreditCustomer
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class CustomerCredited : DomainEvent
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

    public class OrderStatusUpdated// : DomainEvent
    {
        public long OrderId { get; set; }
        public int Status { get; set; }
    }

    public class Rev
    {
        public Guid CustomerId { get; set; }
        public long OrderId { get; set; }
    }

}
