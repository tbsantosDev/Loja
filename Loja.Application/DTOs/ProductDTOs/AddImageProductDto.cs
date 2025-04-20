using Loja.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.ProductDTOs
{
    public class AddImageProductDto
    {
        [Required(ErrorMessage = "A URL da imagem é obrigatória.")]
        public Byte[] Url { get; set; }
        [Required(ErrorMessage = "A sequência da imagem é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A sequência da imagem deve ser um número positivo.")]
        public int Sequence { get; set; }

        [Required(ErrorMessage = "O produto associado à imagem é obrigatório.")]
        public int ProductId { get; set; }
    }
}
