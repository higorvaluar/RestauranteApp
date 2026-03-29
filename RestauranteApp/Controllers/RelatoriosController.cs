using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.ViewModels;

namespace RestauranteApp.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _context;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Relatorios
        public IActionResult Index()
        {
            var vm = new RelatoriosViewModel();
            return View(vm);
        }

        // POST: Relatorios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RelatoriosViewModel vm)
        {
            if (vm.DataFinal < vm.DataInicial)
            {
                ModelState.AddModelError("", "A data final não pode ser menor que a data inicial.");
                return View(vm);
            }

            var dataInicial = vm.DataInicial.Date;
            var dataFinal = vm.DataFinal.Date.AddDays(1).AddTicks(-1);

            vm.FaturamentoPorTipo = await _context.Pedidos
                .Include(p => p.Atendimento)
                .Where(p => p.DataPedido >= dataInicial && p.DataPedido <= dataFinal)
                .GroupBy(p => p.Atendimento != null ? p.Atendimento.Tipo : "Sem Atendimento")
                .Select(g => new FaturamentoPorTipoViewModel
                {
                    TipoAtendimento = g.Key,
                    TotalFaturado = g.Sum(p => p.Total)
                })
                .OrderBy(x => x.TipoAtendimento)
                .ToListAsync();

            var itens = await _context.PedidoItens
                .Include(pi => pi.Pedido)
                .Include(pi => pi.Produto)
                .Where(pi => pi.Pedido.DataPedido >= dataInicial && pi.Pedido.DataPedido <= dataFinal)
                .Select(pi => new
                {
                    pi.ProdutoId,
                    NomeProduto = pi.Produto.Nome,
                    pi.Quantidade,
                    DataPedido = pi.Pedido.DataPedido,
                    PeriodoPedido = pi.Pedido.Periodo
                })
                .ToListAsync();

            var sugestoes = await _context.SugestoesChefe.ToListAsync();

            vm.ItensMaisVendidos = itens
                .Select(i => new
                {
                    i.NomeProduto,
                    i.Quantidade,
                    ComSugestao = sugestoes.Any(s =>
                        s.ProdutoId == i.ProdutoId &&
                        s.Data.Date == i.DataPedido.Date &&
                        s.Periodo == i.PeriodoPedido)
                })
                .GroupBy(x => new { x.NomeProduto, x.ComSugestao })
                .Select(g => new ItemMaisVendidoViewModel
                {
                    NomeProduto = g.Key.NomeProduto,
                    ComSugestaoChefe = g.Key.ComSugestao,
                    QuantidadeVendida = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .ThenBy(x => x.NomeProduto)
                .ToList();

            return View(vm);
        }
    }
}