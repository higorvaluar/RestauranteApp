const estado = {
  dataReserva: null,
  horaReserva: null,
  quantidadePessoas: 2,
  mesaId: null,
  horarioFuncionamento: {
    inicio: 19,
    fim: 22
  }
};

const tabelas = [
  { id: 1, numero: 1, capacidade: 2 },
  { id: 2, numero: 2, capacidade: 2 },
  { id: 3, numero: 3, capacidade: 4 },
  { id: 4, numero: 4, capacidade: 4 },
  { id: 5, numero: 5, capacidade: 6 },
  { id: 6, numero: 6, capacidade: 6 },
  { id: 7, numero: 7, capacidade: 8 },
  { id: 8, numero: 8, capacidade: 8 },
  { id: 9, numero: 9, capacidade: 10 },
  { id: 10, numero: 10, capacidade: 10 }
];

function getUserReservations() {
  return getUserStorageItem("reservas", []);
}

function saveUserReservations(reservas) {
  setUserStorageItem("reservas", reservas);
}

function gerarHorarios() {
  const container = document.getElementById("horarios-disponiveis");
  container.innerHTML = "";

  for (let hora = estado.horarioFuncionamento.inicio; hora < estado.horarioFuncionamento.fim; hora++) {
    const horaStr = `${String(hora).padStart(2, "0")}:00`;

    const btn = document.createElement("button");
    btn.type = "button";
    btn.className = "horario-btn";
    btn.textContent = horaStr;
    btn.onclick = () => selecionarHorario(horaStr, btn);

    container.appendChild(btn);
  }
}

function selecionarHorario(hora, elemento) {
  document.querySelectorAll(".horario-btn.active").forEach(el => el.classList.remove("active"));
  elemento.classList.add("active");
  estado.horaReserva = hora;
  estado.mesaId = null;
  document.querySelectorAll(".mesa-btn.active").forEach(el => el.classList.remove("active"));
  if (estado.dataReserva) {
    carregarMesasDisponiveis();
  }
}

function carregarMesasDisponiveis() {
  const container = document.getElementById("mesas-disponiveis");

  if (!estado.dataReserva || !estado.horaReserva) {
    container.innerHTML = '<p class="text-muted">Selecione data e horário primeiro...</p>';
    return;
  }

  const reservas = getUserReservations();
  const mesasReservadas = reservas
    .filter(r => r.dataReserva === estado.dataReserva && r.horaReserva === estado.horaReserva)
    .map(r => r.mesaId);

  const mesasDisponiveis = tabelas.filter(mesa => !mesasReservadas.includes(mesa.id));

  if (mesasDisponiveis.length === 0) {
    container.innerHTML = '<p class="text-danger fw-bold">Nenhuma mesa disponível para este horário</p>';
    return;
  }

  container.innerHTML = "";

  mesasDisponiveis.forEach(mesa => {
    const temCapacidade = mesa.capacidade >= estado.quantidadePessoas;
    const btn = document.createElement("button");
    btn.type = "button";
    btn.className = "mesa-btn";
    btn.textContent = `Mesa ${mesa.numero}\n(${mesa.capacidade} lugares)`;
    btn.disabled = !temCapacidade;
    btn.title = temCapacidade ? "Clique para selecionar" : `Capacidade insuficiente (${mesa.capacidade} lugares)`;
    btn.onclick = () => selecionarMesa(mesa.id, btn);
    container.appendChild(btn);
  });
}

function selecionarMesa(mesaId, elemento) {
  document.querySelectorAll(".mesa-btn.active").forEach(el => el.classList.remove("active"));
  elemento.classList.add("active");
  estado.mesaId = mesaId;
}

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
  if (estado.dataReserva && estado.horaReserva) {
    carregarMesasDisponiveis();
  }
}

function criarReserva() {
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

  const mesas = tabelas.filter(mesa => mesa.id === estado.mesaId);
  if (!mesas.length) {
    mostrarMensagem("Mesa inválida.", "danger");
    return;
  }

  const reservas = getUserReservations();
  const novaReserva = {
    id: Date.now(),
    dataReserva: estado.dataReserva,
    horaReserva: estado.horaReserva,
    quantidadePessoas: estado.quantidadePessoas,
    mesaId: estado.mesaId,
    mesa: mesas[0],
    codigoConfirmacao: `RES${Math.floor(100000 + Math.random() * 900000)}`,
    dataCriacao: new Date().toISOString()
  };

  reservas.push(novaReserva);
  saveUserReservations(reservas);

  mostrarMensagem(
    `<strong>Reserva confirmada! 🎉</strong><br>
     Código: <div class="codigo-confirmacao" onclick="copiarCodigo(this)">${novaReserva.codigoConfirmacao}</div>
     <small class="text-muted d-block mt-2">Clique no código para copiar</small>`,
    "success"
  );

  document.getElementById("data-reserva").value = "";
  document.getElementById("quantidade-pessoas").value = "2";
  estado.dataReserva = null;
  estado.horaReserva = null;
  estado.quantidadePessoas = 2;
  estado.mesaId = null;
  document.querySelectorAll(".horario-btn.active").forEach(el => el.classList.remove("active"));
  document.querySelectorAll(".mesa-btn.active").forEach(el => el.classList.remove("active"));
  document.getElementById("mesas-disponiveis").innerHTML = '<p class="text-muted">Selecione data e horário primeiro...</p>';

  carregarMinhasReservas();
}

function carregarMinhasReservas() {
  const reservas = getUserReservations();
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

  reservas.slice().reverse().forEach(reserva => {
    const dataFormatada = formatarData(reserva.dataReserva);
    const horarioFormatado = `${reserva.horaReserva}`;
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
}

function cancelarReserva(id) {
  if (!confirm("Tem certeza que deseja cancelar esta reserva?")) {
    return;
  }

  const reservas = getUserReservations().filter(reserva => reserva.id !== id);
  saveUserReservations(reservas);
  mostrarMensagem("Reserva cancelada com sucesso!", "success");
  carregarMinhasReservas();
}

function formatarData(dataStr) {
  const data = new Date(dataStr + "T00:00:00");
  return data.toLocaleDateString("pt-BR", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric"
  }).replace(/^\w/, c => c.toUpperCase());
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

function inicializarReservas() {
  const usuario = requireLogin();
  if (!usuario) return;

  document.getElementById("usuario-nome-reserva").textContent = usuario.nome;
  document.getElementById("data-reserva").addEventListener("change", e => {
    estado.dataReserva = e.target.value;
    carregarMesasDisponiveis();
  });
  document.getElementById("quantidade-pessoas").addEventListener("change", e => {
    estado.quantidadePessoas = Number(e.target.value);
    carregarMesasDisponiveis();
  });

  gerarHorarios();
  atualizarQuantidadePessoas();
  carregarMinhasReservas();
}

window.addEventListener("DOMContentLoaded", inicializarReservas);
