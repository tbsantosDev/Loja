using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.UserDTOs
{
    public class ResetUserPasswordDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Token obrigatório.")]
        public string Token { get; set; }
        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string NewPassword { get; set; }
    }
}
