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
            CarregarCombos();

            var reserva = new Reserva
            {
                DataReserva = DateTime.Today.AddDays(1).AddHours(19),
                QuantidadePessoas = 1
            };

            return View(reserva);
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataReserva,QuantidadePessoas,ClienteId,MesaId")] Reserva reserva)
        {
            await ValidarReservaAsync(reserva);

            if (!ModelState.IsValid)
            {
                CarregarCombos(reserva.ClienteId, reserva.MesaId);
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

            CarregarCombos(reserva.ClienteId, reserva.MesaId);
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

            await ValidarReservaAsync(reserva, id);

            if (!ModelState.IsValid)
            {
                CarregarCombos(reserva.ClienteId, reserva.MesaId);
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

        private void CarregarCombos(int? clienteId = null, int? mesaId = null)
        {
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                clienteId);

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

            // Antecedência mínima de 1 dia
            if (reserva.DataReserva.Date <= DateTime.Today)
            {
                ModelState.AddModelError("DataReserva", "A reserva deve ser feita com pelo menos 1 dia de antecedência.");
            }

            // Janela de horário do jantar: 19h às 22h
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