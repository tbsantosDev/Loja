using Loja.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class ShipmentModel
    {
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "O código de rastreamento deve ter no máximo 50 caracteres.")]
        public string? TrackingCode { get; set; }
        [StringLength(100, ErrorMessage = "O nome da transportadora deve ter no máximo 100 caracteres.")]
        public string? Carrier { get; set; }
        [Required(ErrorMessage = "O status do envio é obrigatório.")]
        [EnumDataType(typeof(ShipmentEnum), ErrorMessage = "Status do envio inválido.")]
        public ShipmentEnum Status { get; set; }

        //chave estrangeira para pedido
        public OrderModel Order { get; set; }
        [Required(ErrorMessage = "O pedido associado ao envio é obrigatório.")]
        public int OrderId { get; set; }
    }
}
