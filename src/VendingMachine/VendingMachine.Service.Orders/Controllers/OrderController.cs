using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Service.Machines.ServiceCommunications.Client.Services;
using VendingMachine.Service.Orders.Application.ViewModels;
using VendingMachine.Service.Orders.Infrastructure.Commands;
using VendingMachine.Service.Orders.Read.Models;
using VendingMachine.Service.Orders.Read.Queries;
using VendingMachine.Service.Products.ServiceCommunications;
using VendingMachine.Service.Products.ServiceCommunications.Client.Services;

namespace VendingMachine.Service.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IOrderQuery orderQuery;
        private readonly IMachineClientService machineClient;
        private readonly IProductItemClientService productItemClient;

        public OrderController(IMediator mediator, IOrderQuery orderQuery,
            IMachineClientService machineClient, IProductItemClientService productItemClient)
        {
            this.mediator = mediator;
            this.orderQuery = orderQuery;
            // Couple to 2 Service
            this.machineClient = machineClient;
            this.productItemClient = productItemClient;
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
                var result = await orderQuery.GetOrder(orderId);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostOrder([FromBody] OrderAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                #region CheckDataAndCreateCommand
                MachineStatusResponse machineStatus;
                ICollection<ProductItemsServiceModel> productItems;
                bool machineExist = await machineClient.ExistMachineAsync(model.MachineId);
                if (machineExist)
                {
                    machineStatus = await machineClient.GetMachineStatus(model.MachineId);
                    var machineInfo = await machineClient.GetMachineInfoAsync(model.MachineId);
                    foreach (var pi in model.ProductItems)
                    {
                        bool productItemExist = await productItemClient.ExistProductItemAsync(pi);
                        if (!productItemExist)
                            ModelState.AddModelError("ProductItemExist", $"Product Item [{pi}] not exist");
                        if (machineInfo.Machine.ActiveProducts.FirstOrDefault(x => x.Id == pi) == null)
                            ModelState.AddModelError("ProductItemNotInMachine", $"Product Item [{pi}] not present in machine");
                    }
                    if (ModelState.IsValid)
                    {
                        productItems = await productItemClient.GetProductItems(model.ProductItems.ToList());
                        foreach (var pi in productItems)
                        {
                            if(pi.Purchased?.ToDateTimeOffset() != default)
                                ModelState.AddModelError("ProductItemPurchased", $"Product Item [{pi.Id}] already purchased");
                        }

                        if (!ModelState.IsValid)
                            return BadRequest(ModelState);
                    }                        
                    else
                        return BadRequest(ModelState);
                }
                else
                {
                    ModelState.AddModelError("MachineExist", $"Machine Id [{model.MachineId}] not exist");
                    return BadRequest(ModelState);
                }
                #endregion

                DateTimeOffset orderDate = DateTimeOffset.UtcNow;
                var orderAddCommand = new OrderAddCommand
                {
                    MachineStatus = new MachineStatusCommand
                    {
                        MachineId = machineStatus.MachineId,
                        CoinsCurrentSupply = (decimal)machineStatus.CoinCurrentSupply
                    },
                    OrderProducts = productItems.Select(x =>
                       new OrderProductItemCommand
                       {
                           ProductItemId = x.Id,
                           Price = new GrossPriceCommand
                           {
                               GrossPrice = (decimal)x.SoldPrice.GrossPrice,
                               TaxPercentage = x.SoldPrice.TaxPercentage
                           }
                       })
                    .ToList(),
                    OrderDate = orderDate
                };
                try
                {
                    var orderAddResponse = await mediator.Send(orderAddCommand);
                    return Ok(new OrderAddedViewModel
                    {
                        CanConfirm = orderAddResponse.CanConfirm,
                        OrderId = orderAddResponse.OrderId
                    });
                }
                catch (InvalidOperationException ioe)
                {
                    ModelState.AddModelError("PendingOrder", ioe.Message);
                    return Conflict(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{orderId:int}/Confirm")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmOrder([FromRoute] int orderId)
        {
            if (orderId > 0)
            {
                OrderConfirmResponse response = await mediator.Send(new OrderConfirmCommand()
                {
                    OrderId = orderId
                });

                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{orderId:int}/AddProductItem/{productItem:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddProductItem([FromRoute] OrderUpdateProductItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new OrderAddProductItemsCommand
                {
                    OrderId = model.OrderId,
                    OrderProductItemsToAdd = new[] { 
                        new OrderProductItemBaseCommand { ProductItemId = model.ProductItem } 
                    }
                };

                var result = await mediator.Send(command);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{orderId:int}/RemoveProductItem/{productItem:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveProductItem([FromRoute] OrderUpdateProductItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new OrderRemoveProductItemsCommand
                {
                    OrderId = model.OrderId,
                    OrderProductItemsToRemove = new[] {
                        new OrderProductItemBaseCommand { ProductItemId = model.ProductItem }
                    }
                };

                var result = await mediator.Send(command);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearProductItems([FromRoute] int orderId)
        {
            if (orderId > 0)
            {
                var command = new OrderClearProductItemsCommand
                {
                    OrderId = orderId
                };

                var result = await mediator.Send(command);

                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(OrderAddedViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOrder([FromRoute] int orderId)
        {
            if (orderId > 0)
            {
                var command = new OrderDeleteCommand
                {
                    OrderId = orderId
                };

                var result = await mediator.Send(command);

                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}