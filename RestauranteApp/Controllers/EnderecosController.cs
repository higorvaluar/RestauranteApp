using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class EnderecosController : Controller
    {
        private readonly AppDbContext _context;

        public EnderecosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Enderecos
        public async Task<IActionResult> Index()
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var enderecos = await _context.Enderecos
                .Where(e => e.ClienteId == clienteId.Value)
                .OrderBy(e => e.Cidade)
                .ThenBy(e => e.Rua)
                .ToListAsync();

            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            return View(enderecos);
        }

        // GET: Enderecos/Create
        public IActionResult Create()
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            return View(new Endereco());
        }

        // POST: Enderecos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rua,Numero,Cidade,Estado,CEP")] Endereco endereco)
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            endereco.ClienteId = clienteId.Value;

            if (!ModelState.IsValid)
            {
                ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
                return View(endereco);
            }

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Enderecos/Edit/5
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

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.ClienteId == clienteId.Value);

            if (endereco == null)
            {
                return NotFound();
            }

            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            return View(endereco);
        }

        // POST: Enderecos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rua,Numero,Cidade,Estado,CEP")] Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return NotFound();
            }

            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            endereco.ClienteId = clienteId.Value;

            if (!ModelState.IsValid)
            {
                ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
                return View(endereco);
            }

            try
            {
                _context.Update(endereco);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool existe = await _context.Enderecos.AnyAsync(e => e.Id == endereco.Id && e.ClienteId == clienteId.Value);
                if (!existe)
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Enderecos/Delete/5
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

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.ClienteId == clienteId.Value);

            if (endereco == null)
            {
                return NotFound();
            }

            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            return View(endereco);
        }

        // POST: Enderecos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.ClienteId == clienteId.Value);

            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}