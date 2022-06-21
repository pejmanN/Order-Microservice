using Framework.Core.Events;
using Framework.Domain;
using OrderManagement.Domain.Contracts;

namespace OrderManagement.Domain.Order
{
    public class Order : AggregateRootBase<long>
    {
        private List<OrderLine> _orderLines;
        public long CustomerId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();

        protected Order() { }
        public Order(long id, long customerId, DateTime issueDate, List<OrderLine> orderLines, IEventAggregator publisher) : base(publisher)
        {
            this.Id = id;
            CustomerId = customerId;
            IssueDate = issueDate;

            if (!orderLines.Any())
            {
                throw new InvalidDataException("OrdeLines should be more than zero");
            }

            this._orderLines = orderLines;

            Publish(new OrderSubmitted(this.Id, this.CustomerId, this.IssueDate, ToOrderLineEvent(orderLines)));

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
