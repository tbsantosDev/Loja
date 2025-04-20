using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.ProductDTOs
{
    public class FavoriteProductDto
    {
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }
    }
}
