using Framework.Core.Events;

namespace OrderManagement.Domain.Contracts
{
    public class OrderSubmitted : DomainEvent
    {
        public OrderSubmitted(long id, long customerId, DateTime issueDate, List<OrderLine> orderLines)
        {
            Id = id;
            CustomerId = customerId;
            IssueDate = issueDate;
            OrderLines = orderLines;
        }

        public long Id { get; private set; }
        public long CustomerId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public List<OrderLine> OrderLines { get; private set; }
    }

    public class OrderLine
    {
        public OrderLine(long productId, long quantity, decimal eachPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            EachPrice = eachPrice;
        }

        public long ProductId { get; private set; }
        public long Quantity { get; private set; }
        public decimal EachPrice { get; private set; }
    }
}
