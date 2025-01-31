using Framework.Core.Events;
using Framework.Domain;
using OrderManagement.Domain.Contracts;
using System.Security.Principal;

namespace OrderManagement.Domain.Order
{
    public class Order : AggregateRootBase<long>
    {
        private List<OrderLine> _orderLines;
        public Guid CustomerId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public OrderStatus Status { get; set; }
        public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();

        protected Order() { }
        public Order(long id, Guid customerId, DateTime issueDate, List<OrderLine> orderLines,
            IEventAggregator publisher, Guid correltionId)
            : base(publisher)
        {
            if (!orderLines.Any())
            {
                throw new InvalidDataException("OrdeLines should be more than zero");
            }

            this.Id = id;
            CustomerId = customerId;
            IssueDate = issueDate;
            _orderLines = orderLines;
            Status = OrderStatus.Submitted;

            Publish(new OrderSubmitted(this.Id, this.CustomerId, this.IssueDate, ToOrderLineEvent(orderLines), correltionId));

        }

        private List<Contracts.OrderLine> ToOrderLineEvent(List<OrderLine> orderLines)
        {
            var result = new List<Contracts.OrderLine>();
            foreach (var line in orderLines)
            {
                result.Add(new Contracts.OrderLine(line.ProductId, line.Quantity, line.EachPrice));
            }

            return result;
        }

    }
}
