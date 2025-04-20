using Loja.Application.DTOs.OrderDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WireMock.Admin.Mappings;

namespace Loja.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderInterface _orderInterface;

        public OrderController(IOrderInterface orderInterface)
        {
            _orderInterface = orderInterface;
        }

        [HttpPost("AddItemToOrder")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> AddItemToOrder(AddItemDto addItemDto)
        {
            try
            {
                var addItemToOrder = await _orderInterface.AddItemToOrder(addItemDto);
                return Ok(addItemToOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CancelOrder/{id}")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> CancelOrder(int id)
        {
            try
            {
                var cancelOrder = await _orderInterface.CancelOrder(id);
                return Ok(cancelOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ConfirmOrderPayment/{orderId}")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> ConfirmOrderPayment(int orderId)
        {
            try
            {
                var confirmOrderPayment = await _orderInterface.ConfirmOrderPayment(orderId);
                return Ok(confirmOrderPayment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var createOrder = await _orderInterface.CreateOrder(createOrderDto);
                return Ok(createOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> DeleteOrder(int id)
        {
            try
            {
                var deleteOrder = await _orderInterface.DeleteOrder(id);
                return Ok(deleteOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMyRecentOrders")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetMyRecentOrders()
        {
            try
            {
                var getMyRecentOrders = await _orderInterface.GetMyRecentOrders();
                return Ok(getMyRecentOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> GetOrderById(int id)
        {
            try
            {
                var getOrderById = await _orderInterface.GetOrderById(id);
                return Ok(getOrderById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrdersByUserId/{userId}")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetOrdersByUserId(int userId)
        {
            try
            {
                var getOrdersByUserId = await _orderInterface.GetOrdersByUserId(userId);
                return Ok(getOrdersByUserId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrderItem")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetOrderItem()
        {
            try
            {
                var getOrderItem = await _orderInterface.GetOrderItem();
                return Ok(getOrderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrders")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetOrders()
        {
            try
            {
                var getOrders = await _orderInterface.GetOrders();
                return Ok(getOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrdersByDateRange")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetOrdersByDateRange(DateTime dateIni, DateTime dateFim)
        {
            try
            {
                var getOrdersByDateRange = await _orderInterface.GetOrdersByDateRange(dateIni, dateFim);
                return Ok(getOrdersByDateRange);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrdersByStatus")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetOrdersByStatus(OrderStatus status)
        {
            try
            {
                var getOrdersByStatus = await _orderInterface.GetOrdersByStatus(status);
                return Ok(getOrdersByStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrderTotal")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> GetOrderTotal(int orderId)
        {
            try
            {
                var getOrderTotal = await _orderInterface.GetOrderTotal(orderId);
                return Ok(getOrderTotal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPendingOrders")]
        public async Task<ActionResult<ResponseModel<List<OrderModel>>>> GetPendingOrders()
        {
            try
            {
                var getPendingOrders = await _orderInterface.GetPendingOrders();
                return Ok(getPendingOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveItemFromOrder/{itemId}")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> RemoveItemFromOrder(int itemId)
        {
            try
            {
                var removeItemFromOrder = await _orderInterface.RemoveItemFromOrder(itemId);
                return Ok(removeItemFromOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateItemInOrder")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> UpdateItemInOrder(UpdateItemDto updateItemDto)
        {
            try
            {
                var updateItemInOrder = await _orderInterface.UpdateItemInOrder(updateItemDto);
                return Ok(updateItemInOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateOrderStatus")]
        public async Task<ActionResult<ResponseModel<OrderModel>>> UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            try
            {
                var updateOrderStatus = await _orderInterface.UpdateOrderStatus(updateOrderStatusDto);
                return Ok(updateOrderStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
