using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A nota da avaliação é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
        public int Grade { get; set; }
        [StringLength(500, ErrorMessage = "O comentário deve ter no máximo 500 caracteres.")]
        public string? Comment { get; set; }
        [Required(ErrorMessage = "A data da avaliação é obrigatória.")]
        public DateTime EvaluationDate { get; set; } = DateTime.UtcNow;

        //Chave estrangeira para Usuario
        [Required(ErrorMessage = "O usuário da avaliação é obrigatório.")]
        public UserModel User { get; set; }
        public int UserId { get; set; }

        //Chave estrangeira para Produto
        [Required(ErrorMessage = "O produto avaliado é obrigatório.")]
        public ProductModel Product { get; set; }
        public int ProductId { get; set; }
    }
}
