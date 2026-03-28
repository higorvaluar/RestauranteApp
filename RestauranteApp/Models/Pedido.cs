namespace RestauranteApp.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public PeriodoEnum Periodo { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public List<PedidoItem> PedidoItens { get; set; } = new();
        public Atendimento? Atendimento { get; set; }  
    }
}