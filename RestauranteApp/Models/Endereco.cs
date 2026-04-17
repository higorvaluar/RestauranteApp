using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string Rua { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número é obrigatório.")]
        [RegularExpression(@"^[A-Za-z0-9À-ÖØ-öø-ÿ\s\-\/\.]+$", ErrorMessage = "O número/complemento contém caracteres inválidos.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]+$", ErrorMessage = "A cidade não pode conter números.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]+$", ErrorMessage = "O estado não pode conter números.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "O CEP deve conter exatamente 8 números.")]
        public string CEP { get; set; } = string.Empty;

        public int ClienteId { get; set; }

        [ValidateNever]
        public Cliente Cliente { get; set; } = null!;
    }
}