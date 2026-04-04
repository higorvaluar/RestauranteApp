const API_BASE = "https://localhost:7189/api";

// Estado da aplicação
const estado = {
  dataReserva: null,
  horaReserva: null,
  quantidadePessoas: 2,
  mesaId: null,
  horarioFuncionamento: null
};

// Inicializar ao carregar a página
document.addEventListener("DOMContentLoaded", async () => {
  try {
    // Carregar horário de funcionamento
    await carregarHorarioFuncionamento();
    
    // Gerar opções de horário
    gerarHorarios();
    
    // Carregar minhas reservas
    await carregarMinhasReservas();

    // Listener para quando alterar a data
    document.getElementById("data-reserva").addEventListener("change", async (e) => {
      estado.dataReserva = e.target.value;
      await carregarMesasDisponiveis();
    });
  } catch (erro) {
    console.error("Erro ao inicializar:", erro);
    mostrarMensagem("Erro ao carregar dados. Tente recarregar a página.", "danger");
  }
});

// ============= HORÁRIOS =============

async function carregarHorarioFuncionamento() {
  try {
    const response = await fetch(`${API_BASE}/reservasapi/horarios`);
    if (!response.ok) throw new Error("Erro ao carregar horários");
    
    const dados = await response.json();
    estado.horarioFuncionamento = {
      inicio: dados.horaInicio,
      fim: dados.horaFim
    };
  } catch (erro) {
    console.error("Erro ao carregar horários:", erro);
    mostrarMensagem("Erro ao carregar horários disponíveis", "danger");
  }
}

function gerarHorarios() {
  const container = document.getElementById("horarios-disponiveis");
  container.innerHTML = "";

  if (!estado.horarioFuncionamento) return;

  const { inicio, fim } = estado.horarioFuncionamento;

  for (let hora = inicio; hora < fim; hora++) {
    const horaStr = `${String(hora).padStart(2, "0")}:00`;
    
    const btn = document.createElement("button");
    btn.type = "button";
    btn.className = "horario-btn";
    btn.textContent = horaStr;
    btn.onclick = () => selecionarHorario(horaStr, btn);
    
    container.appendChild(btn);
  }
}

async function selecionarHorario(hora, elemento) {
  // Remover seleção anterior
  document.querySelectorAll(".horario-btn.active").forEach(el => el.classList.remove("active"));
  
  // Adicionar seleção atual
  elemento.classList.add("active");
  estado.horaReserva = hora;
  
  // Limpar seleção de mesa
  estado.mesaId = null;
  document.querySelectorAll(".mesa-btn.active").forEach(el => el.classList.remove("active"));
  
  // Se data foi selecionada, carregar mesas disponíveis
  if (estado.dataReserva) {
    await carregarMesasDisponiveis();
  }
}

// ============= MESAS =============

async function carregarMesasDisponiveis() {
  if (!estado.dataReserva || !estado.horaReserva) {
    document.getElementById("mesas-disponiveis").innerHTML = 
      '<p class="text-muted">Selecione data e horário primeiro...</p>';
    return;
  }

  try {
    const container = document.getElementById("mesas-disponiveis");
    container.innerHTML = '<div class="spinner-border text-danger" role="status"><span class="visually-hidden">Carregando...</span></div>';

    // Converter hora "HH:00" para "HH:mm"
    const [horaNum] = estado.horaReserva.split(":");
    const horaFormatada = `${horaNum}:00`;

    const response = await fetch(
      `${API_BASE}/reservasapi/mesas-disponiveis?data=${estado.dataReserva}&hora=${horaFormatada}`
    );

    if (!response.ok) {
      throw new Error("Erro ao carregar mesas");
    }

    const mesas = await response.json();

    container.innerHTML = "";

    if (mesas.length === 0) {
      container.innerHTML = '<p class="text-danger fw-bold">Nenhuma mesa disponível para este horário</p>';
      return;
    }

    mesas.forEach(mesa => {
      // Verificar se a mesa tem capacidade suficiente
      const temCapacidade = mesa.capacidade >= estado.quantidadePessoas;

      const btn = document.createElement("button");
      btn.type = "button";
      btn.className = "mesa-btn";
      btn.textContent = `Mesa ${mesa.numero}\n(${mesa.capacidade} lugares)`;
      btn.disabled = !temCapacidade;
      btn.title = temCapacidade 
        ? `Clique para selecionar` 
        : `Capacidade insuficiente (${mesa.capacidade} lugares)`;
      
      btn.onclick = () => selecionarMesa(mesa.id, btn);

      container.appendChild(btn);
    });
  } catch (erro) {
    console.error("Erro ao carregar mesas:", erro);
    document.getElementById("mesas-disponiveis").innerHTML = 
      '<p class="text-danger">Erro ao carregar mesas disponíveis</p>';
  }
}

function selecionarMesa(mesaId, elemento) {
  // Remover seleção anterior
  document.querySelectorAll(".mesa-btn.active").forEach(el => el.classList.remove("active"));
  
  // Adicionar seleção atual
  elemento.classList.add("active");
  estado.mesaId = mesaId;
}

// ============= QUANTIDADE DE PESSOAS =============

function aumentarPessoas() {
  if (estado.quantidadePessoas < 10) {
    estado.quantidadePessoas++;
    atualizarQuantidadePessoas();
  }
}

function diminuirPessoas() {
  if (estado.quantidadePessoas > 1) {
    estado.quantidadePessoas--;
    atualizarQuantidadePessoas();
  }
}

function atualizarQuantidadePessoas() {
  document.getElementById("quantidade-pessoas").value = estado.quantidadePessoas;
  
  // Recarregar mesas se já temos data e hora selecionadas
  if (estado.dataReserva && estado.horaReserva) {
    carregarMesasDisponiveis();
  }
}

// ============= CRIAR RESERVA =============

async function criarReserva() {
  // Validações
  if (!estado.dataReserva) {
    mostrarMensagem("Por favor, selecione uma data", "warning");
    return;
  }

  if (!estado.horaReserva) {
    mostrarMensagem("Por favor, selecione um horário", "warning");
    return;
  }

  if (!estado.mesaId) {
    mostrarMensagem("Por favor, selecione uma mesa", "warning");
    return;
  }

  try {
    const btn = document.getElementById("btn-reservar");
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Processando...';

    const payload = {
      data: estado.dataReserva,
      hora: estado.horaReserva,
      quantidadePessoas: estado.quantidadePessoas,
      mesaId: estado.mesaId
    };

    const response = await fetch(`${API_BASE}/reservasapi`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      credentials: "include",
      body: JSON.stringify(payload)
    });

    const resultado = await response.json();

    if (!response.ok) {
      throw new Error(resultado.mensagem || "Erro ao criar reserva");
    }

    // Sucesso!
    mostrarMensagem(
      `<strong>Reserva confirmada! 🎉</strong><br>
       Código: <div class="codigo-confirmacao" onclick="copiarCodigo(this)">${resultado.codigoConfirmacao}</div>
       <small class="text-muted d-block mt-2">Clique no código para copiar</small>`,
      "success"
    );

    // Limpar formulário
    document.getElementById("data-reserva").value = "";
    document.getElementById("quantidade-pessoas").value = "2";
    estado.quantidadePessoas = 2;
    estado.dataReserva = null;
    estado.horaReserva = null;
    estado.mesaId = null;
    document.querySelectorAll(".horario-btn.active").forEach(el => el.classList.remove("active"));
    document.querySelectorAll(".mesa-btn.active").forEach(el => el.classList.remove("active"));
    document.getElementById("mesas-disponiveis").innerHTML = '<p class="text-muted">Selecione data e horário primeiro...</p>';

    // Recarregar minhas reservas
    setTimeout(() => carregarMinhasReservas(), 1000);
  } catch (erro) {
    console.error("Erro ao criar reserva:", erro);
    mostrarMensagem(erro.message || "Erro ao criar reserva", "danger");
  } finally {
    const btn = document.getElementById("btn-reservar");
    btn.disabled = false;
    btn.innerHTML = '<i class="fas fa-check-circle"></i> Confirmar Reserva';
  }
}

// ============= MINHAS RESERVAS =============

async function carregarMinhasReservas() {
  try {
    const response = await fetch(`${API_BASE}/reservasapi`, {
      credentials: "include"
    });

    if (!response.ok) {
      if (response.status === 401) {
        window.location.href = "login.html";
        return;
      }
      throw new Error("Erro ao carregar reservas");
    }

    const reservas = await response.json();
    const container = document.getElementById("reservas-container");

    if (reservas.length === 0) {
      container.innerHTML = `
        <div class="alert alert-info alert-custom">
          <i class="fas fa-info-circle"></i> Você ainda não tem reservas. 
          <a href="#nova-reserva">Crie uma agora!</a>
        </div>
      `;
      return;
    }

    container.innerHTML = "";

    reservas.forEach(reserva => {
      const dataFormatada = formatarData(reserva.dataReserva);
      const horarioFormatado = `${reserva.horaReserva}:00`;
      const dataCriacao = new Date(reserva.dataCriacao).toLocaleDateString("pt-BR");

      const card = document.createElement("div");
      card.className = "card reserva-card mb-3";
      card.innerHTML = `
        <div class="card-body">
          <div class="row">
            <div class="col-md-6">
              <h5 class="card-title mb-3">
                <i class="fas fa-calendar"></i> ${dataFormatada}
              </h5>
              <div class="mb-3">
                <strong>Horário:</strong> ${horarioFormatado}<br>
                <strong>Pessoas:</strong> ${reserva.quantidadePessoas}<br>
                <strong>Mesa:</strong> ${reserva.mesa.numero} (${reserva.mesa.capacidade} lugares)
              </div>
              <small class="text-muted">
                Criada em: ${dataCriacao}
              </small>
            </div>

            <div class="col-md-6">
              <div class="codigo-confirmacao" onclick="copiarCodigo(this)">
                ${reserva.codigoConfirmacao}
              </div>
              <small class="text-muted d-block mt-2 text-center">
                Clique para copiar
              </small>
              <div class="mt-3">
                <button 
                  class="btn btn-sm btn-outline-danger w-100"
                  onclick="cancelarReserva(${reserva.id})"
                >
                  <i class="fas fa-trash"></i> Cancelar
                </button>
              </div>
            </div>
          </div>
        </div>
      `;

      container.appendChild(card);
    });
  } catch (erro) {
    console.error("Erro ao carregar reservas:", erro);
    document.getElementById("reservas-container").innerHTML = `
      <div class="alert alert-danger alert-custom">
        <i class="fas fa-exclamation-circle"></i> Erro ao carregar suas reservas
      </div>
    `;
  }
}

async function cancelarReserva(id) {
  if (!confirm("Tem certeza que deseja cancelar esta reserva?")) {
    return;
  }

  try {
    const response = await fetch(`${API_BASE}/reservasapi/${id}`, {
      method: "DELETE",
      credentials: "include"
    });

    const resultado = await response.json();

    if (!response.ok) {
      throw new Error(resultado.mensagem || "Erro ao cancelar reserva");
    }

    mostrarMensagem(resultado.mensagem, "success");
    setTimeout(() => carregarMinhasReservas(), 500);
  } catch (erro) {
    console.error("Erro ao cancelar:", erro);
    mostrarMensagem(erro.message || "Erro ao cancelar reserva", "danger");
  }
}

// ============= UTILITÁRIOS =============

function formatarData(dataStr) {
  const data = new Date(dataStr + "T00:00:00");
  return data.toLocaleDateString("pt-BR", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric"
  }).replace(/^\w/, (c) => c.toUpperCase());
}

function mostrarMensagem(mensagem, tipo) {
  const container = document.getElementById("msg-container");
  const alert = document.createElement("div");
  alert.className = `alert alert-${tipo} alert-custom alert-dismissible fade show`;
  alert.role = "alert";
  alert.innerHTML = `
    ${mensagem}
    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Fechar"></button>
  `;
  
  container.innerHTML = "";
  container.appendChild(alert);

  // Auto-remover após 5 segundos
  setTimeout(() => {
    alert.classList.remove("show");
    setTimeout(() => alert.remove(), 150);
  }, 5000);
}

function copiarCodigo(elemento) {
  const codigo = elemento.textContent.trim();
  navigator.clipboard.writeText(codigo).then(() => {
    const textoOriginal = elemento.textContent;
    elemento.textContent = "✓ Copiado!";
    elemento.style.background = "linear-gradient(135deg, #2e7d32 0%, #558b2f 100%)";
    
    setTimeout(() => {
      elemento.textContent = textoOriginal;
      elemento.style.background = "";
    }, 2000);
  }).catch(() => {
    alert("Erro ao copiar. Copie manualmente: " + codigo);
  });
}
