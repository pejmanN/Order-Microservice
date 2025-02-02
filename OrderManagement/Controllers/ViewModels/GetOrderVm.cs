using OrderManagement.Domain.Order;

namespace OrderManagement.ViewModels
{
    public class GetOrderVm
    {
        public Guid CustomerId { get; set; }
        public DateTime IssueDate { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderLineVm> OrderLines { get; set; }
        public Guid CorrelaitonId { get; set; }

    }

    public class OrderLineVm
    {
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public decimal EachPrice { get; set; }
    }
}
