using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Service.Orders.Application.ViewModels;
using VendingMachine.Service.Orders.Infrastructure.Commands;
using VendingMachine.Service.Orders.Read.Models;
using VendingMachine.Service.Orders.Read.Queries;

namespace VendingMachine.Service.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IOrderQuery orderQuery;

        public OrderController(IMediator mediator, IOrderQuery orderQuery)
        {
            this.mediator = mediator;
            this.orderQuery = orderQuery;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders([FromQuery] PagedRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await orderQuery.GetOrders(model.ToReadModel());
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            if (ModelState.IsValid)
            {
                //orderQuery

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostOrder([FromBody] OrderAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTimeOffset orderDate = DateTimeOffset.UtcNow;
                var orderAddCommand = new OrderAddCommand
                {
                    MachineStatus = new MachineStatusCommand
                    {
                        MachineId = model.ModelStatus.MachineId,
                        CoinsCurrentSupply = model.ModelStatus.CoinCurrentSupply
                    },
                    OrderProducts = model.ProductItems.Select(x =>
                        new OrderProductItemCommand
                        {
                            ProductItemId = x.ProductItem,
                            Price = new GrossPriceCommand
                            {
                                GrossPrice = x.Price.GrossPrice,
                                TaxPercentage = x.Price.TaxPercentage
                            }
                        }),
                    OrderDate = orderDate
                };

                var orderAddResponse = await mediator.Send(orderAddCommand);
                return Ok(new OrderAddedViewModel
                {
                    CanConfirm = orderAddResponse.CanConfirm,
                    OrderId = orderAddResponse.OrderId
                });
            }
            return BadRequest(ModelState);
        }
    }
}