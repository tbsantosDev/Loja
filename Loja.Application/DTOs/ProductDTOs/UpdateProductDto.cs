using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.ProductDTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "O estoque do produto é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
        public int Stock { get; set; }
        [Range(1000, 2100, ErrorMessage = "O ano deve estar entre 1000 e 2100.")]
        public int? Year { get; set; }

        [StringLength(100, ErrorMessage = "A origem deve ter no máximo 100 caracteres.")]
        public string? Origin { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O peso não pode ser negativo.")]
        public int? Weight { get; set; }

        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        public int CategoryId { get; set; }
    }
}
