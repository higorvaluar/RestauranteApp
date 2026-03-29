using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class ReservasController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .OrderBy(r => r.DataReserva)
                .ToListAsync();

            return View(reservas);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            CarregarCombos();

            var reserva = new Reserva
            {
                ClienteId = clienteId.Value,
                DataReserva = DateTime.Today.AddDays(1).AddHours(19),
                QuantidadePessoas = 1
            };

            ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
            return View(reserva);
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataReserva,QuantidadePessoas,ClienteId,MesaId")] Reserva reserva)
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            reserva.ClienteId = clienteId.Value;

            await ValidarReservaAsync(reserva);

            if (!ModelState.IsValid)
            {
                CarregarCombos(reserva.MesaId);
                ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
                return View(reserva);
            }

            _context.Add(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            CarregarCombos(reserva.MesaId);
            ViewBag.ClienteNome = reserva.Cliente?.Nome ?? HttpContext.Session.GetString("ClienteNome");
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataReserva,QuantidadePessoas,ClienteId,MesaId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            var clienteId = HttpContext.Session.GetInt32("ClienteId");
            if (clienteId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            reserva.ClienteId = clienteId.Value;

            await ValidarReservaAsync(reserva, id);

            if (!ModelState.IsValid)
            {
                CarregarCombos(reserva.MesaId);
                ViewBag.ClienteNome = HttpContext.Session.GetString("ClienteNome");
                return View(reserva);
            }

            try
            {
                _context.Update(reserva);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(reserva.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private void CarregarCombos(int? mesaId = null)
        {
            ViewData["MesaId"] = new SelectList(
                _context.Mesas.OrderBy(m => m.Numero),
                "Id",
                "Numero",
                mesaId);
        }

        private async Task ValidarReservaAsync(Reserva reserva, int? idIgnorar = null)
        {
            if (reserva.QuantidadePessoas <= 0)
            {
                ModelState.AddModelError("QuantidadePessoas", "A quantidade de pessoas deve ser maior que zero.");
            }

            if (reserva.DataReserva.Date <= DateTime.Today)
            {
                ModelState.AddModelError("DataReserva", "A reserva deve ser feita com pelo menos 1 dia de antecedência.");
            }

            var hora = reserva.DataReserva.TimeOfDay;
            var inicio = new TimeSpan(19, 0, 0);
            var fim = new TimeSpan(22, 0, 0);

            if (hora < inicio || hora > fim)
            {
                ModelState.AddModelError("DataReserva", "A reserva deve ser feita no horário do jantar (19:00 às 22:00).");
            }

            var mesa = await _context.Mesas.FindAsync(reserva.MesaId);

            if (mesa == null)
            {
                ModelState.AddModelError("MesaId", "Mesa inválida.");
                return;
            }

            if (reserva.QuantidadePessoas > mesa.Capacidade)
            {
                ModelState.AddModelError("QuantidadePessoas", "A quantidade de pessoas excede a capacidade da mesa.");
            }

            var query = _context.Reservas.AsQueryable();

            if (idIgnorar.HasValue)
            {
                query = query.Where(r => r.Id != idIgnorar.Value);
            }

            bool mesaJaReservada = await query.AnyAsync(r =>
                r.MesaId == reserva.MesaId &&
                r.DataReserva == reserva.DataReserva);

            if (mesaJaReservada)
            {
                ModelState.AddModelError("", "Já existe uma reserva para essa mesa nesse mesmo horário.");
            }
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}