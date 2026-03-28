namespace RestauranteApp.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public DateTime DataReserva { get; set; }
        public int QuantidadePessoas { get; set; }
        public string CodigoConfirmacao { get; set; } = string.Empty;

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public int MesaId { get; set; }
        public Mesa Mesa { get; set; } = null!;
    }
}