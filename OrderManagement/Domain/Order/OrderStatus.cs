namespace OrderManagement.Domain.Order
{
    public enum OrderStatus
    {
        Submitted = 3,
        Accepted = 4,
        ItemGranted = 5,
        Canceled = 6,
        Faulted = 7,
        Completed = 8,
    }
}
