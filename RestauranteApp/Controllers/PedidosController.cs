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
        private const decimal TAXA_FIXA_DELIVERY_PROPRIO = 8.00m;

        public PedidosController(AppDbContext context, PedidoService pedidoService)
        {
            _context = context;
            _pedidoService = pedidoService;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var admin = UsuarioAdmin();

            var query = _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Atendimento)
                .Include(p => p.PedidoItens)
                    .ThenInclude(pi => pi.Produto)
                .AsQueryable();

            if (!admin)
            {
                query = query.Where(p => p.ClienteId == clienteId.Value);
            }

            var pedidos = await query
                .OrderByDescending(p => p.DataPedido)
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

            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var admin = UsuarioAdmin();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Atendimento)
                .Include(p => p.PedidoItens)
                    .ThenInclude(pi => pi.Produto)
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    (admin || m.ClienteId == clienteId.Value));

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (UsuarioAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var vm = new PedidoCreateViewModel
            {
                DataPedido = DateTime.Now,
                Periodo = PeriodoEnum.Almoco,
                ClienteId = clienteId.Value,
                TipoAtendimento = "Presencial",
                Itens = CriarItensVazios(3)
            };

            CarregarCombos(clienteId.Value);
            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            ViewBag.TaxaFixaDeliveryProprio = TAXA_FIXA_DELIVERY_PROPRIO.ToString("F2");
            return View(vm);
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoCreateViewModel vm)
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (UsuarioAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            vm.ClienteId = clienteId.Value;
            vm.DataPedido = DateTime.Now;

            GarantirItens(vm, 3);
            CarregarCombos(clienteId.Value);
            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            ViewBag.TaxaFixaDeliveryProprio = TAXA_FIXA_DELIVERY_PROPRIO.ToString("F2");

            var itensValidos = vm.Itens
                .Where(i => i.ProdutoId.HasValue && i.ProdutoId.Value > 0 && i.Quantidade > 0)
                .ToList();

            if (!itensValidos.Any())
            {
                ModelState.AddModelError("", "O pedido precisa ter pelo menos um item válido.");
            }

            Endereco? enderecoSelecionado = null;

            if (vm.TipoAtendimento == "DeliveryProprio")
            {
                if (!vm.EnderecoId.HasValue || vm.EnderecoId.Value <= 0)
                {
                    ModelState.AddModelError("EnderecoId", "Selecione um endereço de entrega.");
                }
                else
                {
                    enderecoSelecionado = await _context.Enderecos
                        .FirstOrDefaultAsync(e => e.Id == vm.EnderecoId.Value && e.ClienteId == clienteId.Value);

                    if (enderecoSelecionado == null)
                    {
                        ModelState.AddModelError("EnderecoId", "Endereço de entrega inválido.");
                    }
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
                DataPedido = DateTime.Now,
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
                    EnderecoEntrega = FormatarEndereco(enderecoSelecionado!),
                    TaxaEntrega = TAXA_FIXA_DELIVERY_PROPRIO
                },
                "DeliveryAplicativo" => new AtendimentoDeliveryAplicativo
                {
                    Tipo = "Delivery Aplicativo",
                    NomeAplicativo = vm.NomeAplicativo!.Trim()
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

            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var admin = UsuarioAdmin();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Atendimento)
                .Include(p => p.PedidoItens)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    (admin || p.ClienteId == clienteId.Value));

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

            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var admin = UsuarioAdmin();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    (admin || m.ClienteId == clienteId.Value));

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
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var admin = UsuarioAdmin();

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    (admin || p.ClienteId == clienteId.Value));

            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void CarregarCombos(int clienteId)
        {
            ViewBag.Produtos = _context.Produtos
                .AsNoTracking()
                .OrderBy(p => p.Nome)
                .ToList();

            var enderecos = _context.Enderecos
                .Where(e => e.ClienteId == clienteId)
                .OrderBy(e => e.Cidade)
                .ThenBy(e => e.Rua)
                .Select(e => new
                {
                    e.Id,
                    Texto = e.Rua + ", " + e.Numero + " - " + e.Cidade + "/" + e.Estado + " - CEP " + e.CEP
                })
                .ToList();

            ViewBag.Enderecos = new SelectList(enderecos, "Id", "Texto");
        }

        private static string FormatarEndereco(Endereco endereco)
        {
            return $"{endereco.Rua}, {endereco.Numero} - {endereco.Cidade}/{endereco.Estado} - CEP {endereco.CEP}";
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

        private bool UsuarioAdmin()
        {
            return HttpContext.Session.GetInt32("Admin") == 1;
        }
    }
}