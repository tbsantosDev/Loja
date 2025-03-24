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
        [Required(ErrorMessage = "A senha Atual é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Defina uma nova senha.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirme a nova senha escolhida.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string ConfirmPassword { get; set; }
    }
}
