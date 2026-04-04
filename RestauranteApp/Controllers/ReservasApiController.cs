using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;
using RestauranteApp.Services;
using System.Security.Claims;

namespace RestauranteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ReservaService _reservaService;

        public ReservasApiController(AppDbContext context, ReservaService reservaService)
        {
            _context = context;
            _reservaService = reservaService;
        }

        /// <summary>
        /// GET: api/reservas - Listar todas as reservas do usuário autenticado
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetReservas()
        {
            var clienteId = GetClienteIdFromSession();
            if (clienteId == null)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var reservas = await _context.Reservas
                .Include(r => r.Mesa)
                .Include(r => r.Cliente)
                .Where(r => r.ClienteId == clienteId.Value)
                .OrderByDescending(r => r.DataReserva)
                .Select(r => new
                {
                    r.Id,
                    r.DataReserva,
                    r.HoraReserva,
                    r.QuantidadePessoas,
                    r.CodigoConfirmacao,
                    r.DataCriacao,
                    Mesa = new { r.Mesa.Numero, r.Mesa.Capacidade }
                })
                .ToListAsync();

            return Ok(reservas);
        }

        /// <summary>
        /// GET: api/reservas/{id} - Obter detalhes de uma reserva específica
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetReserva(int id)
        {
            var clienteId = GetClienteIdFromSession();
            if (clienteId == null)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var reserva = await _context.Reservas
                .Include(r => r.Mesa)
                .Include(r => r.Cliente)
                .FirstOrDefaultAsync(r => r.Id == id && r.ClienteId == clienteId.Value);

            if (reserva == null)
            {
                return NotFound(new { mensagem = "Reserva não encontrada" });
            }

            return Ok(new
            {
                reserva.Id,
                reserva.DataReserva,
                reserva.HoraReserva,
                reserva.QuantidadePessoas,
                reserva.CodigoConfirmacao,
                reserva.DataCriacao,
                Mesa = new { reserva.Mesa.Numero, reserva.Mesa.Capacidade },
                Cliente = new { reserva.Cliente.Nome, reserva.Cliente.Email, reserva.Cliente.Telefone }
            });
        }

        /// <summary>
        /// POST: api/reservas - Criar uma nova reserva
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateReserva([FromBody] CreateReservaRequest request)
        {
            var clienteId = GetClienteIdFromSession();
            if (clienteId == null)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            // Validar request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Converter strings para tipos apropriados
            if (!DateOnly.TryParse(request.Data, out var data))
            {
                return BadRequest(new { mensagem = "Formato de data inválido (use yyyy-MM-dd)" });
            }

            if (!TimeOnly.TryParse(request.Hora, out var hora))
            {
                return BadRequest(new { mensagem = "Formato de hora inválido (use HH:mm)" });
            }

            // Criar objeto reserva
            var reserva = new Reserva
            {
                DataReserva = data,
                HoraReserva = hora,
                QuantidadePessoas = request.QuantidadePessoas,
                ClienteId = clienteId.Value,
                MesaId = request.MesaId
            };

            // Validar e criar a reserva
            var (sucesso, mensagem, reservaCriada) = await _reservaService.CriarReservaAsync(reserva);

            if (!sucesso)
            {
                return BadRequest(new { mensagem });
            }

            return CreatedAtAction(
                nameof(GetReserva),
                new { id = reservaCriada!.Id },
                new
                {
                    reservaCriada.Id,
                    reservaCriada.DataReserva,
                    reservaCriada.HoraReserva,
                    reservaCriada.QuantidadePessoas,
                    reservaCriada.CodigoConfirmacao,
                    mensagem = "Reserva criada com sucesso!"
                }
            );
        }

        /// <summary>
        /// PUT: api/reservas/{id} - Atualizar uma reserva
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateReserva(int id, [FromBody] UpdateReservaRequest request)
        {
            var clienteId = GetClienteIdFromSession();
            if (clienteId == null)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.Id == id && r.ClienteId == clienteId.Value);

            if (reserva == null)
            {
                return NotFound(new { mensagem = "Reserva não encontrada" });
            }

            // Converter strings
            if (!DateOnly.TryParse(request.Data, out var data))
            {
                return BadRequest(new { mensagem = "Formato de data inválido" });
            }

            if (!TimeOnly.TryParse(request.Hora, out var hora))
            {
                return BadRequest(new { mensagem = "Formato de hora inválido" });
            }

            // Atualizar dados
            reserva.DataReserva = data;
            reserva.HoraReserva = hora;
            reserva.QuantidadePessoas = request.QuantidadePessoas;
            reserva.MesaId = request.MesaId;

            // Revalidar
            var (sucesso, mensagem, _) = await _reservaService.CriarReservaAsync(reserva);
            if (!sucesso)
            {
                return BadRequest(new { mensagem });
            }

            _context.Update(reserva);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Reserva atualizada com sucesso!", reserva.Id, reserva.CodigoConfirmacao });
        }

        /// <summary>
        /// DELETE: api/reservas/{id} - Cancelar uma reserva
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteReserva(int id)
        {
            var clienteId = GetClienteIdFromSession();
            if (clienteId == null)
            {
                return Unauthorized(new { mensagem = "Usuário não autenticado" });
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.Id == id && r.ClienteId == clienteId.Value);

            if (reserva == null)
            {
                return NotFound(new { mensagem = "Reserva não encontrada" });
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Reserva cancelada com sucesso!" });
        }

        /// <summary>
        /// GET: api/reservas/mesas-disponiveis?data=2026-04-10&hora=20:00 - Obter mesas disponíveis
        /// </summary>
        [HttpGet("mesas-disponiveis")]
        public async Task<ActionResult<IEnumerable<object>>> GetMesasDisponiveis([FromQuery] string data, [FromQuery] string hora)
        {
            if (!DateOnly.TryParse(data, out var dataReserva))
            {
                return BadRequest(new { mensagem = "Formato de data inválido" });
            }

            if (!TimeOnly.TryParse(hora, out var horaReserva))
            {
                return BadRequest(new { mensagem = "Formato de hora inválido" });
            }

            var mesas = await _reservaService.ObterMesasDisponiveisAsync(dataReserva, horaReserva);

            return Ok(mesas.Select(m => new
            {
                m.Id,
                m.Numero,
                m.Capacidade
            }).ToList());
        }

        /// <summary>
        /// GET: api/reservas/horarios - Obter horários de funcionamento
        /// </summary>
        [HttpGet("horarios")]
        public ActionResult<object> GetHorarios()
        {
            var (horaInicio, horaFim) = _reservaService.ObterHorarioFuncionamentoReservas();

            return Ok(new
            {
                horaInicio,
                horaFim,
                mensagem = $"Reservas disponíveis das {horaInicio}h às {horaFim}h"
            });
        }

        // Helper: Obter ClienteId da sessão
        private int? GetClienteIdFromSession()
        {
            return HttpContext.Session.GetInt32("ClienteId");
        }
    }

    // DTOs para requisições
    public class CreateReservaRequest
    {
        public string Data { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public int QuantidadePessoas { get; set; }
        public int MesaId { get; set; }
    }

    public class UpdateReservaRequest
    {
        public string Data { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public int QuantidadePessoas { get; set; }
        public int MesaId { get; set; }
    }
}
