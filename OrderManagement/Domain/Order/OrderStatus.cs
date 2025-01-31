namespace OrderManagement.Domain.Order
{
    public enum OrderStatus : byte
    {
        Submitted,
        Accepted,
        Canceled,
        Faulted,
        Completed,
    }
}
