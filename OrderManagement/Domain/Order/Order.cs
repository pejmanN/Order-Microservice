using Framework.Core.Events;
using Framework.Domain;
using OrderManagement.Domain.Contracts;
using System.Linq.Expressions;
using System.Security.Principal;

namespace OrderManagement.Domain.Order
{
    public class Order : AggregateRootBase<long>
    {
        private List<OrderLine> _orderLines;
        public Guid CustomerId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public OrderStatus Status { get; set; }
        public Guid CorrelationId { get; private set; }
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
            CorrelationId = correltionId;

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

        public void SetStatus(int status)
        {
            if (Enum.IsDefined(typeof(OrderStatus), status))
            {
                this.Status = (OrderStatus)status;
            }
            else
            {
                throw new InvalidCastException($"Provided Satus is not valid, Status= {status}");
            }
        }

    }
}
