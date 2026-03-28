namespace RestauranteApp.Models
{
    public abstract class Atendimento
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;
        public decimal TaxaEntrega { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public abstract decimal CalcularTaxa(decimal subtotalItens, PeriodoEnum periodo);
    }
}