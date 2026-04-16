using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RestauranteApp.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public DateTime DataReserva { get; set; }
        public int QuantidadePessoas { get; set; }
        public string? CodigoConfirmacao { get; set; }

        public int ClienteId { get; set; }

        [ValidateNever]
        public Cliente Cliente { get; set; } = null!;

        public int MesaId { get; set; }

        [ValidateNever]
        public Mesa Mesa { get; set; } = null!;
    }
}