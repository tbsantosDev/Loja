using Loja.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Loja.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; } 
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O número de telefone não é válido.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [EnumDataType(typeof(UserEnum), ErrorMessage = "Tipo de usuário inválido.")]
        public UserEnum UserType { get; set; }
        public bool Approved { get; set; } = false;
        [Required(ErrorMessage = "A data de criação do usuário é obrigatório.")]
        public DateTime CreatedAt { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
        [JsonIgnore]

        public ICollection<OrderModel> Orders { get; set; } = [];
        [JsonIgnore]
        public ICollection<FavoriteModel> Favorites { get; set; } = [];
        [JsonIgnore]
        public ICollection<ReviewModel> Reviews { get; set; } = [];
    }
}
