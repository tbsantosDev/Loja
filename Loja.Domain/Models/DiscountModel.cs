using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class DiscountModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O percentual de desconto é obrigatório.")]
        [Range(0, 100, ErrorMessage = "O percentual de desconto deve estar entre 0% e 100%.")]
        public decimal Percentage { get; set; }
        [Required(ErrorMessage = "A data de início do desconto é obrigatória.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "A data de término do desconto é obrigatória.")]
        [DataType(DataType.DateTime, ErrorMessage = "Data de término inválida.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "O status do desconto é obrigatório.")]
        public bool Active { get; set; } = false;

        //chave estrangeira opcional para produto
        public ProductModel Product { get; set; }
        public int? ProductId { get; set; }

        //chave estrangeira opcional para categoria
        public CategoryModel Category { get; set; }
        public int? CategoryId { get; set; }
    }
}
