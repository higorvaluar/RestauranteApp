using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data da reserva é obrigatória")]
        public DateOnly DataReserva { get; set; }

        [Required(ErrorMessage = "Horário é obrigatório")]
        public TimeOnly HoraReserva { get; set; }

        [Required(ErrorMessage = "Quantidade de pessoas é obrigatória")]
        [Range(1, 10, ErrorMessage = "Deve ser entre 1 e 10 pessoas")]
        public int QuantidadePessoas { get; set; }

        public string? CodigoConfirmacao { get; set; }

        [Required(ErrorMessage = "Cliente é obrigatório")]
        public int ClienteId { get; set; }

        [ValidateNever]
        public Cliente Cliente { get; set; } = null!;

        [Required(ErrorMessage = "Mesa é obrigatória")]
        public int MesaId { get; set; }

        [ValidateNever]
        public Mesa Mesa { get; set; } = null!;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}