using Framework.Domain;

namespace OrderManagement.Domain.Order
{
    public class OrderLine : ValueObjectBase
    {
        public long ProductId { get; private set; }
        public long Quantity { get; private set; }
        public decimal EachPrice { get; private set; }
        protected OrderLine() { }
        public OrderLine(long productId, long quantity, decimal eachPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            EachPrice = eachPrice;
        }
    }
}
