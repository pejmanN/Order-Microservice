namespace Shared.StateMachines.Order.Models
{
    public interface ValidateCustomer
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface CustomerValidated
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface AllocateInventory
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface InventorAllocated
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface DebitCustomer
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface CustomerDebited
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface CreditCustomer
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface CustomerCredited
    {
        Guid CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public class OrderStatusUpdated
    {
        public long OrderId { get; set; }
        public int Status { get; set; }
    }

}
