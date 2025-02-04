namespace OrderManagement.Domain.Order
{
    public enum OrderStatus
    {
        Submitted = 2,
        Accepted = 3,
        Validated = 4,
        ItemGranted = 5,
        Canceled = 6,
        Faulted = 7,
        Completed = 8,
    }
}
