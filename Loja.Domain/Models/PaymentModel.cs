using Loja.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;


namespace Loja.Domain.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O status do pagamento é obrigatório.")]
        [EnumDataType(typeof(PaymentEnumStatus), ErrorMessage = "Status do pagamento inválido.")]
        public PaymentEnumStatus Status { get; set; }
        [Required(ErrorMessage = "A data e hora do pagamento são obrigatórias.")]
        public DateTime PaymentDateTime { get; set; }
        [Required(ErrorMessage = "O método de pagamento é obrigatório.")]
        [EnumDataType(typeof(PaymentMethodOrder), ErrorMessage = "Método de pagamento inválido.")]
        public PaymentMethodOrder PaymentMethod { get; set; }
        [Required(ErrorMessage = "O valor do pagamento é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do pagamento deve ser maior que zero.")]
        public decimal Value { get; set; }

        //chave estrangeira para pedido
        public OrderModel Order { get; set; }
        [Required(ErrorMessage = "O pedido associado ao pagamento é obrigatório.")]
        public int OrderId { get; set; }

    }
}
