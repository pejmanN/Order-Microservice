namespace OrderManagement.VireModels
{
    public class SubmitOrderVM
    {
        public int CustomerId { get; set; }
        public List<OrderLineVM> OrderLines { get; set; }
    }

    public class OrderLineVM
    {
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public decimal EachPrice { get; set; }
    }
}
