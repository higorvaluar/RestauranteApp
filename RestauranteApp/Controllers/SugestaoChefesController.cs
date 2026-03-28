using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class SugestaoChefesController : Controller
    {
        private readonly AppDbContext _context;

        public SugestaoChefesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SugestaoChefes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.SugestoesChefe.Include(s => s.Produto);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SugestaoChefes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sugestaoChefe = await _context.SugestoesChefe
                .Include(s => s.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sugestaoChefe == null)
            {
                return NotFound();
            }

            return View(sugestaoChefe);
        }

        // GET: SugestaoChefes/Create
        public IActionResult Create()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome");
            return View();
        }

        // POST: SugestaoChefes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Periodo,PercentualDesconto,ProdutoId")] SugestaoChefe sugestaoChefe)
        {
            await ValidarSugestaoChefe(sugestaoChefe);

            if (!ModelState.IsValid)
            {
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", sugestaoChefe.ProdutoId);
                return View(sugestaoChefe);
            }

            try
            {
                _context.Add(sugestaoChefe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível salvar. Verifique se já existe uma sugestão para essa data e período.");
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", sugestaoChefe.ProdutoId);
                return View(sugestaoChefe);
            }
        }

        // GET: SugestaoChefes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sugestaoChefe = await _context.SugestoesChefe.FindAsync(id);
            if (sugestaoChefe == null)
            {
                return NotFound();
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", sugestaoChefe.ProdutoId);
            return View(sugestaoChefe);
        }

        // POST: SugestaoChefes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Periodo,PercentualDesconto,ProdutoId")] SugestaoChefe sugestaoChefe)
        {
            if (id != sugestaoChefe.Id)
            {
                return NotFound();
            }

            await ValidarSugestaoChefe(sugestaoChefe, id);

            if (!ModelState.IsValid)
            {
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", sugestaoChefe.ProdutoId);
                return View(sugestaoChefe);
            }

            try
            {
                _context.Update(sugestaoChefe);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SugestaoChefeExists(sugestaoChefe.Id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível salvar. Verifique se já existe uma sugestão para essa data e período.");
                ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", sugestaoChefe.ProdutoId);
                return View(sugestaoChefe);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SugestaoChefes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sugestaoChefe = await _context.SugestoesChefe
                .Include(s => s.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sugestaoChefe == null)
            {
                return NotFound();
            }

            return View(sugestaoChefe);
        }

        // POST: SugestaoChefes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sugestaoChefe = await _context.SugestoesChefe.FindAsync(id);
            if (sugestaoChefe != null)
            {
                _context.SugestoesChefe.Remove(sugestaoChefe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SugestaoChefeExists(int id)
        {
            return _context.SugestoesChefe.Any(e => e.Id == id);
        }

        private async Task ValidarSugestaoChefe(SugestaoChefe sugestaoChefe, int? idIgnorar = null)
        {
            var produto = await _context.Produtos.FindAsync(sugestaoChefe.ProdutoId);

            if (produto == null)
            {
                ModelState.AddModelError("ProdutoId", "Produto inválido.");
                return;
            }

            if (produto.Periodo != sugestaoChefe.Periodo)
            {
                ModelState.AddModelError("", "O período da sugestão deve ser o mesmo período do produto.");
            }

            var query = _context.SugestoesChefe.AsQueryable();

            if (idIgnorar.HasValue)
            {
                query = query.Where(s => s.Id != idIgnorar.Value);
            }

            var jaExiste = await query.AnyAsync(s =>
                s.Data.Date == sugestaoChefe.Data.Date &&
                s.Periodo == sugestaoChefe.Periodo);

            if (jaExiste)
            {
                ModelState.AddModelError("", "Já existe uma Sugestão do Chefe cadastrada para essa data e período.");
            }

            if (sugestaoChefe.PercentualDesconto != 20)
            {
                ModelState.AddModelError("PercentualDesconto", "O percentual de desconto deve ser 20.");
            }
        }
    }
}