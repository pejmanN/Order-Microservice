
namespace OrderManagement.Facade
{
    public interface IOrderFacadeService
    {
        Task<long> Create(SubmitOrderCommand command);
    }
}