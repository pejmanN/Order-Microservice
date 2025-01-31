using CusomerManagement.Domain.Service;

namespace CusomerManagement.Infra.ACL
{
    public class OrderACLService : IOrderService
    {
        public Order GetOrder(long orderId)
        {
            //Get related info from Order Service
            return new Order { OrderId = orderId, TotalCost = 1000 };
        }
    }
}
