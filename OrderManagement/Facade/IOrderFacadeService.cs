
namespace OrderManagement.Facade
{
    public interface IOrderFacadeService
    {
        Task<Guid> Create(SubmitOrderCommand command);

        Task SetOrderStatus(SetOrderStatusCommand setOrderStatusCommand);
    }
}