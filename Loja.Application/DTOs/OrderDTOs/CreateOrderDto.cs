using Loja.Domain.Models.Enums;
using Loja.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "A data e hora do pedido são obrigatórias.")]
        public DateTime OrderTime { get; set; }
        [Required(ErrorMessage = "O status do pedido é obrigatório.")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Status do pedido inválido.")]
        public OrderStatus Status { get; set; }
        [Required(ErrorMessage = "O valor total do pedido é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero.")]
        public decimal Total { get; set; }
        [Required(ErrorMessage = "O endereço de entrega é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço de entrega deve ter no máximo 200 caracteres.")]
        public string DeliveryAddress { get; set; }
        [Required(ErrorMessage = "O método de pagamento é obrigatório.")]
        [EnumDataType(typeof(PaymentMethodOrder), ErrorMessage = "Método de pagamento inválido.")]
        public PaymentMethodOrder PaymentMethod { get; set; }

        //chave estrangeira com usuario}
        [Required(ErrorMessage = "O usuário associado ao pedido é obrigatório.")]
        public int UserId { get; set; }

    }
}
