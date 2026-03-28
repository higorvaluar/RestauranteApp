namespace RestauranteApp.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<Produto> Produtos { get; set; } = new();
    }
}