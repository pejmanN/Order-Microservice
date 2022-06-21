namespace OrderManagement.Facade
{
    public class SubmitOrderCommand
    {
        public int CustomerId { get; set; }
        public List<OrderLineCommand> OrderLines { get; set; }
    }


    public class OrderLineCommand
    {
        public long ProductId { get;  set; }
        public long Quantity { get;  set; }
        public decimal EachPrice { get;  set; }
    }
}
