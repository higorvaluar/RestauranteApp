using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using RestauranteApp.Services;
using RestauranteApp.ViewModels;

namespace RestauranteApp.Controllers
{
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PedidoService _pedidoService;

        public PedidosController(AppDbContext context, PedidoService pedidoService)
        {
            _context = context;
            _pedidoService = pedidoService;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Atendimento)
                .ToListAsync();

            return View(pedidos);
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Atendimento)
                .Include(p => p.PedidoItens)
                    .ThenInclude(pi => pi.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            var vm = new PedidoCreateViewModel
            {
                DataPedido = DateTime.Now,
                Periodo = PeriodoEnum.Almoco,
                TipoAtendimento = "Presencial",
                Itens = CriarItensVazios(3)
            };

            CarregarCombos();
            return View(vm);
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoCreateViewModel vm)
        {
            GarantirItens(vm, 3);
            CarregarCombos();

            var itensValidos = vm.Itens
                .Where(i => i.ProdutoId.HasValue && i.ProdutoId.Value > 0 && i.Quantidade > 0)
                .ToList();

            if (!itensValidos.Any())
            {
                ModelState.AddModelError("", "O pedido precisa ter pelo menos um item válido.");
            }

            if (vm.TipoAtendimento == "DeliveryProprio")
            {
                if (string.IsNullOrWhiteSpace(vm.EnderecoEntrega))
                {
                    ModelState.AddModelError("EnderecoEntrega", "Informe o endereço de entrega.");
                }

                if (vm.TaxaEntrega < 0)
                {
                    ModelState.AddModelError("TaxaEntrega", "A taxa de entrega não pode ser negativa.");
                }
            }

            if (vm.TipoAtendimento == "DeliveryAplicativo")
            {
                if (string.IsNullOrWhiteSpace(vm.NomeAplicativo))
                {
                    ModelState.AddModelError("NomeAplicativo", "Informe o nome do aplicativo.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var pedido = new Pedido
            {
                DataPedido = vm.DataPedido,
                Periodo = vm.Periodo,
                ClienteId = vm.ClienteId,
                PedidoItens = itensValidos.Select(i => new PedidoItem
                {
                    ProdutoId = i.ProdutoId!.Value,
                    Quantidade = i.Quantidade
                }).ToList()
            };

            pedido.Atendimento = vm.TipoAtendimento switch
            {
                "Presencial" => new AtendimentoPresencial
                {
                    Tipo = "Presencial",
                    TaxaEntrega = 0m
                },
                "DeliveryProprio" => new AtendimentoDeliveryProprio
                {
                    Tipo = "Delivery Próprio",
                    EnderecoEntrega = vm.EnderecoEntrega,
                    TaxaEntrega = vm.TaxaEntrega
                },
                "DeliveryAplicativo" => new AtendimentoDeliveryAplicativo
                {
                    Tipo = "Delivery Aplicativo",
                    NomeAplicativo = vm.NomeAplicativo
                },
                _ => null
            };

            if (pedido.Atendimento == null)
            {
                ModelState.AddModelError("TipoAtendimento", "Tipo de atendimento inválido.");
                return View(vm);
            }

            try
            {
                await _pedidoService.CalcularPedidoAsync(pedido);

                _context.Add(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Atendimento)
                .Include(p => p.PedidoItens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = pedido.Id });
        }

        // POST: Pedidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }

        private void CarregarCombos()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "Id", "Nome");
            ViewBag.Produtos = new SelectList(_context.Produtos.OrderBy(p => p.Nome), "Id", "Nome");
        }

        private static List<PedidoCreateItemViewModel> CriarItensVazios(int quantidade)
        {
            var itens = new List<PedidoCreateItemViewModel>();

            for (int i = 0; i < quantidade; i++)
            {
                itens.Add(new PedidoCreateItemViewModel());
            }

            return itens;
        }

        private static void GarantirItens(PedidoCreateViewModel vm, int quantidadeMinima)
        {
            vm.Itens ??= new List<PedidoCreateItemViewModel>();

            while (vm.Itens.Count < quantidadeMinima)
            {
                vm.Itens.Add(new PedidoCreateItemViewModel());
            }
        }
    }
}