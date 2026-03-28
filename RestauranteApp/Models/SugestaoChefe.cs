namespace RestauranteApp.Models
{
    public class SugestaoChefe
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public PeriodoEnum Periodo { get; set; }

        public decimal PercentualDesconto { get; set; } = 20;

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
    }
}