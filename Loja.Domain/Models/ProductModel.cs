using Loja.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class ProductModel
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

        //chave estrangeira para categoria
        public CategoryModel Category { get; set; }
        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        public int CategoryId { get; set; }

        public ICollection<FavoriteModel> FavoritedBy { get; set; } = [];
        public ICollection<OrderedItemsModel> OrderedIn { get; set; } = [];
        public ICollection<ProductImageModel> ProductImages { get; set; } = [];

        public ICollection<ReviewModel> Reviews { get; set; } = [];

        public decimal PriceForPF => Price;
        public decimal PriceForPJ => Price * 0.90m; // 10% de desconto


        public decimal GetPrice(UserEnum customerType)
        {
            return customerType == UserEnum.businessClient ? PriceForPJ : PriceForPF;
        }
    }
}

