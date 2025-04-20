using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.ProductDTOs
{
    public class AddProductReviewDto
    {
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "A nota da avaliação é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
        public int Grade { get; set; }
        [StringLength(500, ErrorMessage = "O comentário deve ter no máximo 500 caracteres.")]
        public string? Comment { get; set; }
    }
}
