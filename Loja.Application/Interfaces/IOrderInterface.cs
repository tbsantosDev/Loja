using Loja.Application.DTOs.OrderDTOs;
using Loja.Domain.Models;
using Loja.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.Interfaces
{
    public interface IOrderInterface
    {
        Task<ResponseModel<OrderModel>> CreateOrder(CreateOrderDto createOrderDto);
        Task<ResponseModel<OrderModel>> GetOrderById(int id);
        Task<ResponseModel<List<OrderModel>>> GetOrdersByUserId(int userId);
        Task<ResponseModel<List<OrderModel>>> GetPendingOrders();
        Task<ResponseModel<List<OrderModel>>> GetOrdersByDateRange(DateTime dateIni, DateTime dateFim);
        Task<ResponseModel<List<OrderModel>>> GetOrdersByStatus(OrderStatus status);
        Task<ResponseModel<List<OrderModel>>> GetOrderItem();
        Task<ResponseModel<OrderModel>> GetOrderTotal(int orderId);
        Task<ResponseModel<List<OrderModel>>> GetOrders();
        Task<ResponseModel<List<OrderModel>>> GetMyRecentOrders();
        Task<ResponseModel<OrderModel>> UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto);
        Task<ResponseModel<OrderModel>> AddItemToOrder(AddItemDto addItemDto);
        Task<ResponseModel<OrderModel>> RemoveItemFromOrder();
        Task<ResponseModel<OrderModel>> UpdateItemInOrder();
        Task<ResponseModel<OrderModel>> ConfirmOrderPayment();
        Task<ResponseModel<OrderModel>> CancelOrder(int id);
        Task<ResponseModel<OrderModel>> DeleteOrder(int id);
    }
}
