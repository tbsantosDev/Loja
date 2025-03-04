using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A quantidade do produto no carrinho é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Amount { get; set; }

        //chave estrangeira para usuario
        public UserModel User { get; set; }
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int UserId { get; set; }

        //chave estrangeira para produto
        public ProductModel Product { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }
    }
}
