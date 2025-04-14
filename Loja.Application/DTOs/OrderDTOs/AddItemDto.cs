using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.OrderDTOs
{
    public class AddItemDto
    {
        [Required(ErrorMessage = "O pedido é obrigatório.")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        public int Amount { get; set; }
    }
}
