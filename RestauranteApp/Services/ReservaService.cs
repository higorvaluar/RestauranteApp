using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Services
{
    public class ReservaService
    {
        private readonly AppDbContext _context;

        // Horário de funcionamento para reservas (apenas jantar)
        private const int HORA_INICIO = 19;
        private const int HORA_FIM = 22;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Valida se a data está no futuro
        /// </summary>
        public bool ValidarData(DateOnly data)
        {
            return data >= DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        }

        /// <summary>
        /// Valida se a hora está dentro do horário permitido (19h-22h)
        /// </summary>
        public bool ValidarHora(TimeOnly hora)
        {
            return hora.Hour >= HORA_INICIO && hora.Hour < HORA_FIM;
        }

        /// <summary>
        /// Valida se a quantidade de pessoas é compatível com a capacidade da mesa
        /// </summary>
        public async Task<bool> ValidarCapacidadeMesaAsync(int mesaId, int quantidadePessoas)
        {
            var mesa = await _context.Mesas.FindAsync(mesaId);
            if (mesa == null) return false;

            return quantidadePessoas <= mesa.Capacidade;
        }

        /// <summary>
        /// Verifica se já existe uma reserva para a mesma mesa, data e hora
        /// </summary>
        public async Task<bool> VerificarConflitoDuplicidadeAsync(int mesaId, DateOnly data, TimeOnly hora)
        {
            var reservaExistente = await _context.Reservas
                .Where(r => r.MesaId == mesaId && 
                           r.DataReserva == data && 
                           r.HoraReserva == hora)
                .FirstOrDefaultAsync();

            return reservaExistente != null;
        }

        /// <summary>
        /// Gera um código de confirmação único
        /// </summary>
        public string GerarCodigoConfirmacao()
        {
            // Formato: RES-YYYYMMDD-XXXXX
            var data = DateTime.Now.ToString("yyyyMMdd");
            var numero = new Random().Next(10000, 99999);
            return $"RES-{data}-{numero}";
        }

        /// <summary>
        /// Cria uma nova reserva com todas as validações
        /// </summary>
        public async Task<(bool sucesso, string mensagem, Reserva? reserva)> CriarReservaAsync(Reserva reserva)
        {
            // Validar data
            if (!ValidarData(reserva.DataReserva))
            {
                return (false, "A data deve ser no mínimo 1 dia no futuro", null);
            }

            // Validar hora
            if (!ValidarHora(reserva.HoraReserva))
            {
                return (false, $"As reservas são permitidas apenas entre {HORA_INICIO}h e {HORA_FIM}h", null);
            }

            // Validar quantidade de pessoas
            if (reserva.QuantidadePessoas < 1 || reserva.QuantidadePessoas > 10)
            {
                return (false, "Quantidade de pessoas deve estar entre 1 e 10", null);
            }

            // Validar capacidade da mesa
            var mesaValida = await ValidarCapacidadeMesaAsync(reserva.MesaId, reserva.QuantidadePessoas);
            if (!mesaValida)
            {
                return (false, "A mesa selecionada não tem capacidade suficiente para a quantidade de pessoas", null);
            }

            // Verificar conflito de duplicidade
            var temConflito = await VerificarConflitoDuplicidadeAsync(reserva.MesaId, reserva.DataReserva, reserva.HoraReserva);
            if (temConflito)
            {
                return (false, "A mesa já está reservada para este data e horário. Escolha outro horário ou mesa", null);
            }

            // Verificar se cliente existe
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == reserva.ClienteId);
            if (!clienteExiste)
            {
                return (false, "Cliente não encontrado", null);
            }

            // Gerar código de confirmação
            reserva.CodigoConfirmacao = GerarCodigoConfirmacao();
            reserva.DataCriacao = DateTime.Now;

            // Salvar a reserva
            _context.Add(reserva);
            await _context.SaveChangesAsync();

            return (true, "Reserva criada com sucesso!", reserva);
        }

        /// <summary>
        /// Obtém as mesas disponíveis para um determinado data e hora
        /// </summary>
        public async Task<List<Mesa>> ObterMesasDisponiveisAsync(DateOnly data, TimeOnly hora)
        {
            var mesasReservadas = await _context.Reservas
                .Where(r => r.DataReserva == data && r.HoraReserva == hora)
                .Select(r => r.MesaId)
                .ToListAsync();

            return await _context.Mesas
                .Where(m => !mesasReservadas.Contains(m.Id))
                .OrderBy(m => m.Numero)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém o horário de abertura e fechamento para reservas
        /// </summary>
        public (int horaInicio, int horaFim) ObterHorarioFuncionamentoReservas()
        {
            return (HORA_INICIO, HORA_FIM);
        }
    }
}
