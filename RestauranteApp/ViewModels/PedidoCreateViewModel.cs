using System.ComponentModel.DataAnnotations;
using RestauranteApp.Models;

namespace RestauranteApp.ViewModels
{
    public class PedidoCreateItemViewModel
    {
        [Display(Name = "Produto")]
        public int? ProdutoId { get; set; }

        [Display(Name = "Quantidade")]
        [Range(0, 999)]
        public int Quantidade { get; set; }
    }

    public class PedidoCreateViewModel
    {
        [Display(Name = "Data do Pedido")]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Display(Name = "Período")]
        public PeriodoEnum Periodo { get; set; }

        [Display(Name = "Cliente")]
        [Required]
        public int ClienteId { get; set; }

        [Display(Name = "Tipo de Atendimento")]
        [Required]
        public string TipoAtendimento { get; set; } = "Presencial";

        [Display(Name = "Endereço de Entrega")]
        public int? EnderecoId { get; set; }

        [Display(Name = "Aplicativo")]
        public string? NomeAplicativo { get; set; }

        public List<PedidoCreateItemViewModel> Itens { get; set; } = new();
    }
}