using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class ProductImageModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A URL da imagem é obrigatória.")]
        public Byte[] Url { get; set; }
        [Required(ErrorMessage = "A sequência da imagem é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A sequência da imagem deve ser um número positivo.")]
        public int Sequence { get; set; }

        //chave estrangeira para produto
        [Required(ErrorMessage = "O produto associado à imagem é obrigatório.")]
        public ProductModel Product { get; set; }
        public int ProductId { get; set; }
    }
}
