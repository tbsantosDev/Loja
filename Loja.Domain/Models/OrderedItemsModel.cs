using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class OrderedItemsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "O preço unitário é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal UnitPrice { get; set; }

        //chave estrangeira para pedido
        public OrderModel Order { get; set; }
        [Required(ErrorMessage = "O pedido é obrigatório.")]
        public int OrderId { get; set; }

        //chave estrangeira para produto
        public ProductModel Product { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }
    }
}
