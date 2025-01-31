using Microsoft.AspNetCore.Mvc.RazorPages;
using OrderManagement.Domain.Order;

namespace OrderManagement.Infra.Query
{
    public class Order
    {
        public Guid CustomerId { get; set; }
        public DateTime IssueDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public Guid CorrelaitonId { get; set; }
    }

    public class OrderLine
    {
        public long ProductId { get; private set; }
        public long Quantity { get; private set; }
        public decimal EachPrice { get; private set; }
    }
}
