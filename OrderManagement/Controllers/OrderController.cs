﻿using Microsoft.AspNetCore.Mvc;
using OrderManagement.Facade;
using OrderManagement.ViewModels;
using OrderManagement.VireModels;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderFacadeService _orderFacadeService;
        public OrderController(IOrderFacadeService orderFacadeService)
        {
            _orderFacadeService = orderFacadeService;
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public ActionResult<GetOrderVm> GetOrder(int id)
        {
            return null;
        }

        [HttpPost]
        public async Task<ActionResult<GetOrderVm>> Post([FromBody] SubmitOrderVM submitOrderVM)
        {
            throw new Exception("Order service is done!");
            var createdOrderId = await _orderFacadeService.Create(new SubmitOrderCommand
            {
                CustomerId = submitOrderVM.CustomerId,
                OrderLines = ToSubmitOrderCommandOrderLines(submitOrderVM.OrderLines)
            });

            return CreatedAtRoute(nameof(GetOrder), new { id = createdOrderId }, new GetOrderVm());
        }

        private List<OrderLineCommand> ToSubmitOrderCommandOrderLines(List<OrderLineVM> orderLines)
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
