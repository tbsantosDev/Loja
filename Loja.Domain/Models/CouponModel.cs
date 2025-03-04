using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models
{
    public class CouponModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O código do cupom é obrigatório.")]
        [StringLength(20, ErrorMessage = "O código do cupom deve ter no máximo 20 caracteres.")]
        public string Code { get; set; }
        [StringLength(255, ErrorMessage = "A descrição do cupom deve ter no máximo 255 caracteres.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "A porcentagem de desconto é obrigatória.")]
        [Range(0.01, 100, ErrorMessage = "A porcentagem de desconto deve estar entre 0.01% e 100%.")]
        public decimal Percentage { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade máxima de usos deve ser pelo menos 1.")]
        public int? MaximumQuantity { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade de usos realizados não pode ser negativa.")]
        public int UsesPerformed { get; set; } = 0;
        [Required(ErrorMessage = "A data de início do cupom é obrigatória.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "A data de validade do cupom é obrigatória.")]
        [DateGreaterThan("StartDate", ErrorMessage = "A data de validade deve ser posterior à data de início.")]
        public DateTime EndDate { get; set; }
        public bool Active { get; set; } = false;
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                return new ValidationResult($"Propriedade '{_comparisonProperty}' não encontrada.");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (value is DateTime currentValue && currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage ?? "A data final deve ser posterior à data inicial.");

            return ValidationResult.Success;
        }
    }
}
