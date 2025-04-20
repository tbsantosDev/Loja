using Loja.Application.DTOs.OrderDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Domain.Models.Enums;
using Loja.Infra.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.Services
{
    public class OrderService : IOrderInterface
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<ResponseModel<OrderModel>> AddItemToOrder(AddItemDto addItemDto)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == addItemDto.OrderId);

                if (order == null)
                {
                    response.Message = "Pedido não encontrado.";
                    response.Status = false;
                    return response;
                }

                var product = await _context.Products.FindAsync(addItemDto.ProductId);
                if (product == null)
                {
                    response.Message = "Produto não encontrado.";
                    response.Status = false;
                    return response;
                }

                var newItem = new OrderedItemsModel
                {
                    OrderId = addItemDto.OrderId,
                    ProductId = addItemDto.ProductId,
                    Amount = addItemDto.Amount,
                    UnitPrice = product.Price,
                };

                _context.OrderedItems.Add(newItem);
                await GetOrderTotal(order.Id);
                await _context.SaveChangesAsync();

                response.Dados = order;
                response.Message = "Item adicionado ao pedido com sucesso!";
                response.Status = true;
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> CancelOrder(int id)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                var roleClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || roleClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);
                var userRole = roleClaim.Value;


                var cancelOrder = await _context.Orders.FirstOrDefaultAsync(c => c.Id == id);
                if (cancelOrder == null)
                {
                    response.Message = "Pedido não encontrado!";
                    return response;
                }

                if (cancelOrder.UserId != userId && userRole.ToLower() != "admin")
                {
                    response.Message = "Você não tem permissão para cancelar este pedido.";
                    response.Status = false;
                    return response;
                }


                if (cancelOrder.Status == OrderStatus.delivered || cancelOrder.Status == OrderStatus.cancelled)
                {
                    response.Message = "Este pedido não pode ser cancelado!";
                }

                cancelOrder.Status = OrderStatus.cancelled;

                _context.Orders.Update(cancelOrder);
                await _context.SaveChangesAsync();

                response.Dados = cancelOrder;
                response.Message = "Pedido cancelado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex) 
            { 
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        //Serviço temporiario, substituir quando implementar o gateway de pagamento.
        public async Task<ResponseModel<OrderModel>> ConfirmOrderPayment(int orderId)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || userRoleClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);
                var userRole = userRoleClaim.Value;

                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null)
                {
                    response.Message = "Pedido não encontrado.";
                    response.Status = false;
                    return response;
                }

                if (order.UserId != userId && userRole.ToLower() != "admin")
                {
                    response.Message = "Você não tem permissão para confirmar o pagamento deste pedido.";
                    response.Status = false;
                    return response;
                }

                if (order.Status == OrderStatus.paid)
                {
                    response.Message = "Este pedido já está com o pagamento confirmado.";
                    response.Status = false;
                    return response;
                }

                order.Status = OrderStatus.paid;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                response.Dados = order;
                response.Message = "Pagamento confirmado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex) 
            { 
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> CreateOrder(CreateOrderDto createOrderDto)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var createOrder = new OrderModel()
                {
                    OrderTime = DateTime.Now,
                    Status = createOrderDto.Status,
                    Total = 0,
                    DeliveryAddress = createOrderDto.DeliveryAddress,
                    PaymentMethod = createOrderDto.PaymentMethod,
                    UserId = userId,
                };

                _context.Add(createOrder);
                await _context.SaveChangesAsync();

                response.Dados = createOrder;
                response.Message = "Pedido criado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> DeleteOrder(int id)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var deleteOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (deleteOrder == null)
                {
                    response.Message = "Pedido não encontrado.";
                    return response;
                }
                _context.Orders.Remove(deleteOrder);
                await _context.SaveChangesAsync();

                response.Dados = deleteOrder;
                response.Message = "Pedido excluído com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<OrderModel>>> GetMyRecentOrders()
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }
                var userId = int.Parse(userIdClaim.Value);

                var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

                var recentOrders = await _context.Orders
                    .Where(o => o.UserId == userId && o.OrderTime >= oneMonthAgo)
                    .OrderByDescending(o => o.OrderTime)
                    .ToListAsync();

                response.Dados = recentOrders;
                response.Message = "Pedidos mais recentes encontrados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> GetOrderById(int id)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var getOrderById = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (getOrderById == null)
                {
                    response.Message = "Pedido não localizado!";
                    return response;
                }
                response.Dados = getOrderById;
                response.Message = "Pedido localizado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<OrderModel>>> GetOrdersByUserId(int userId)
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var getOrdersByUserId = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
                if (getOrdersByUserId.Count == 0)
                {
                    response.Message = "Nenhum pedido vinculado a este usuário.";
                    return response;
                }
                response.Dados = getOrdersByUserId;
                response.Message = "Pedidos do usuário coletado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<OrderModel>>> GetOrderItem()
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var getOrderItem = await _context.Orders.Include(i => i.Items).ToListAsync();
                if (getOrderItem.Count == 0)
                {
                    response.Message = "Nenhum item vinculado a este pedido.";
                    return response;
                }
                response.Dados = getOrderItem;
                response.Message = "Itens dos pedidos localizado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<OrderModel>>> GetOrders()
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var getOrders = await _context.Orders.ToListAsync();
                if (getOrders.Count == 0)
                {
                    response.Message = "Nenhum pedido localizado!";
                    return response;
                }
                response.Dados = getOrders;
                response.Message = "Pedidos localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<OrderModel>>> GetOrdersByDateRange(DateTime dateIni, DateTime dateFim)
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var getOrdersByDateRange = await _context.Orders.Where(o => o.OrderTime >= dateIni && o.OrderTime <= dateFim).ToListAsync();
                if (getOrdersByDateRange.Count == 0)
                {
                    response.Message = "Não existe pedidos dentro do período selecionado.";
                    return response;
                }
                response.Dados = getOrdersByDateRange;
                response.Message = "Pedidos localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }

        }

        public async Task<ResponseModel<List<OrderModel>>> GetOrdersByStatus(OrderStatus status)
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var getOrdersByStatus = await _context.Orders.Where(o => o.Status == status).ToListAsync();
                if (getOrdersByStatus.Count == 0)
                {
                    response.Message = "Nenhum pedido localizado!";
                    return response;
                }
                response.Dados = getOrdersByStatus;
                response.Message = "Pedidos localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> GetOrderTotal(int orderId)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var getOrder = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == orderId);
                if (getOrder == null)
                {
                    response.Message = "Pedido não localizado!";
                    return response;
                }

                var total = getOrder.Items.Sum(i => i.UnitPrice * i.Amount);

                getOrder.Total = total;

                _context.Orders.Update(getOrder);
                await _context.SaveChangesAsync();

                response.Dados = getOrder;
                response.Message = "Total calculado com sucesso!";
                response.Status = true;
                return response;


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<OrderModel>>> GetPendingOrders()
        {
            ResponseModel<List<OrderModel>> response = new();

            try
            {
                var getPendingOrders = await _context.Orders.Where(o => o.Status == OrderStatus.pending).ToListAsync();
                if (getPendingOrders.Count == 0)
                {
                    response.Message = "Não existem pedidos pendentes.";
                    return response;
                }
                response.Dados = getPendingOrders;
                response.Message = "Pedidos pendentes localizados com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> RemoveItemFromOrder(int itemId)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || userRoleClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);
                var userRole = userRoleClaim.Value;

                var item = await _context.OrderedItems
                    .Include(i => i.Order)
                    .FirstOrDefaultAsync(i => i.Id == itemId);

                if (item == null)
                {
                    response.Message = "Item do pedido não encontrado.";
                    return response;
                }

                var order = item.Order;

                if (order.UserId != userId && userRole.ToLower() != "admin")
                {
                    response.Message = "Você não tem permissão para remover este item.";
                    return response;
                }

                _context.OrderedItems.Remove(item);
                await GetOrderTotal(order.Id);
                await _context.SaveChangesAsync();

                response.Dados = order;
                response.Message = "Item removido e pedido atualizado com sucesso.";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> UpdateItemInOrder(UpdateItemDto updateItemDto)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                var userRoleClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || userRoleClaim == null)
                {
                    response.Message = "Usuário não autenticado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);
                var userRole = userRoleClaim.Value;

                var item = await _context.OrderedItems
                    .Include(i => i.Order)
                    .FirstOrDefaultAsync(i => i.Id == updateItemDto.ItemId);

                if (item == null)
                {
                    response.Message = "Item do pedido não encontrado.";
                    response.Status = false;
                    return response;
                }

                var order = item.Order;

                if (order.UserId != userId && userRole.ToLower() != "admin")
                {
                    response.Message = "Você não tem permissão para atualizar este item.";
                    response.Status = false;
                    return response;
                }

                item.Amount = updateItemDto.Amount;

                _context.OrderedItems.Update(item);
                await GetOrderTotal(order.Id);
                await _context.SaveChangesAsync();

                response.Dados = order;
                response.Message = "Item atualizado e total do pedido recalculado com sucesso.";
                response.Status = true;
                return response;

            }
            catch (Exception ex) 
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrderModel>> UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            ResponseModel<OrderModel> response = new();

            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateOrderStatusDto.Id);
                if (order == null)
                {
                    response.Message = "Pedido não encontrado.";
                    return response;
                }

                order.Status = updateOrderStatusDto.Status;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                response.Dados = order;
                response.Message = "Status do pedido atualizado com sucesso!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}
