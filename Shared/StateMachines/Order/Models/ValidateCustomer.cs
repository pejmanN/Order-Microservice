namespace Shared.StateMachines.Order.Models
{
    public interface ValidateCustomer
    {
        long CustomerId { get; set; }
        long OrderId { get; set; }
    }

    public interface CustomerValidated
    {
        long CustomerId { get; set; }
        long OrderId { get; set; }
    }
}
