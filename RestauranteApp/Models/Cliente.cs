namespace RestauranteApp.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public List<Reserva> Reservas { get; set; } = new();
        public List<Pedido> Pedidos { get; set; } = new();
    }
}