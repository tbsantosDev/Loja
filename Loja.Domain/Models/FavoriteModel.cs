using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class FavoriteModel
    {
        public int Id { get; set; }

        //Chave estrangeira para usuario
        public UserModel User { get; set; }
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int UserId { get; set; }

        //chave estrangeira para produto
        public ProductModel Product { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }
    }
}
