using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.OrderDTOs
{
    public class UpdateItemDto
    {
        [Required(ErrorMessage = "O item do produto é obrigatório.")]
        public int ItemId { get; set; }
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        public int Amount { get; set; }
    }
}
