using OrderManagement.Facade;
using OrderManagement.ViewModels;
using OrderManagement.VireModels;

namespace OrderManagement.Controllers.ViewModels
{
    public static class OrderMapper
    {
        public static GetOrderVm MapToOrderVm(Infra.Query.Order order)
        {          
            return new GetOrderVm
            {
                CustomerId = order.CustomerId,
                IssueDate = order.IssueDate,
                Status = order.Status,
                CorrelaitonId = order.CorrelaitonId,
                OrderLines = order.OrderLines?.Select(MapToVm).ToList() ?? Enumerable.Empty<OrderLineVm>()
            };
        }

        private static OrderLineVm MapToVm(Infra.Query.OrderLine orderLine)
        {
            return new OrderLineVm
            {
                ProductId = orderLine.ProductId,
                Quantity = orderLine.Quantity,
                EachPrice = orderLine.EachPrice
            };
        }

        public static List<OrderLineCommand> ToSubmitOrderCommandOrderLines(List<OrderLineVM> orderLines)
        {
            var result = new List<OrderLineCommand>();
            foreach (var line in orderLines)
            {
                result.Add(new OrderLineCommand
                {
                    EachPrice = line.EachPrice,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity,
                });
            }

            return result;
        }
    }
}
