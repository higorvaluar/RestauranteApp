namespace RestauranteApp.ViewModels
{
    public class FaturamentoPorTipoViewModel
    {
        public string TipoAtendimento { get; set; } = string.Empty;
        public decimal TotalFaturado { get; set; }
    }

    public class ItemMaisVendidoViewModel
    {
        public string NomeProduto { get; set; } = string.Empty;
        public int QuantidadeVendida { get; set; }
        public bool ComSugestaoChefe { get; set; }
    }

    public class RelatoriosViewModel
    {
        public DateTime DataInicial { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime DataFinal { get; set; } = DateTime.Today;

        public List<FaturamentoPorTipoViewModel> FaturamentoPorTipo { get; set; } = new();
        public List<ItemMaisVendidoViewModel> ItensMaisVendidos { get; set; } = new();
    }
}