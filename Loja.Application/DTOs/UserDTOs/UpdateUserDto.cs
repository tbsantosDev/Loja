using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
        public string Address { get; set; }
    }
}
