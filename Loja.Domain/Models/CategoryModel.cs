using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }

        // Relacionamento com produtos
        public ICollection<ProductModel> Products { get; set; } = [];

        // Relacionamento com descontos
        public ICollection<DiscountModel> Discounts { get; set; } = [];
    }
}
