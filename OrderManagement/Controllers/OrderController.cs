using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Controllers.ViewModels;
using OrderManagement.Facade;
using OrderManagement.Facade.Query;
using OrderManagement.Infra.Query;
using OrderManagement.ViewModels;
using OrderManagement.VireModels;
using Sayad.Authorization;
using System.Security.Claims;

namespace OrderManagement.Controllers
{
    [OrderAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderFacadeService _orderFacadeService;
        private readonly IOrderQeueryFacade _orderQueryFacadeService;
        public OrderController(IOrderFacadeService orderFacadeService,
                               IOrderQeueryFacade orderQueryFacadeService)
        {
            _orderFacadeService = orderFacadeService;
            _orderQueryFacadeService = orderQueryFacadeService;
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public ActionResult<GetOrderVm> GetOrder(Guid id)
        {
            var order = _orderQueryFacadeService.GetOrder(id);
            if (order is null)
            {
                return NotFound();
            }
            return Ok(OrderMapper.MapToOrderVm(order));
        }

        [HttpPost]
        public async Task<ActionResult<GetOrderVm>> Post([FromBody] SubmitOrderVM submitOrderVM)
        {
            var customerId = User.FindFirstValue("sub");
            var corrlationId = await _orderFacadeService.Create(new SubmitOrderCommand
            {
                CustomerId = new Guid(customerId),
                OrderLines = OrderMapper.ToSubmitOrderCommandOrderLines(submitOrderVM.OrderLines)
            });

            return AcceptedAtAction(nameof(GetOrder), new { id = corrlationId }, new { CorrelationId = corrlationId });
        }

    }
}
