namespace RestauranteApp.Models
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        public PeriodoEnum Periodo { get; set; }

        public List<Ingrediente> Ingredientes { get; set; } = new();
        public List<SugestaoChefe> SugestoesChefe { get; set; } = new();
    }
}