using RestauranteApp.Models;

namespace RestauranteApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Produto> ProdutosAlmoco { get; set; } = new();
        public List<Produto> ProdutosJantar { get; set; } = new();

        public SugestaoChefe? SugestaoAlmoco { get; set; }
        public SugestaoChefe? SugestaoJantar { get; set; }
    }
}