namespace OrderManagement.Facade.Query
{
    public interface IOrderQeueryFacade
    {
        Infra.Query.Order GetOrder(Guid correlationId);
    }
}
