using Loja.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.OrderDTOs
{
    public class UpdateOrderStatusDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O status do pedido é obrigatório.")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Status do pedido inválido.")]
        public OrderStatus Status { get; set; }
    }
}
