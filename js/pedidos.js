const MENU = {
    almoco: [
        { id: 1, nome: "Yakissoba Vegetariano", preco: 28.90 },
        { id: 2, nome: "Yakisoba Carboidrato", preco: 32.50 },
        { id: 3, nome: "Frango Crocante ao Molho", preco: 31.90 },
        { id: 4, nome: "Lasanha da Casa", preco: 35.00 }
    ],
    jantar: [
        { id: 5, nome: "Risoto de Camarão", preco: 42.90 },
        { id: 6, nome: "Filé Mignon ao Molho", preco: 47.50 },
        { id: 7, nome: "Parmegiana de Frango", preco: 38.90 },
        { id: 8, nome: "Salmão Grelhado", preco: 44.00 }
    ]
};

const TAXA_DELIVERY_PROPRIO = 5.00;
const APP_DELIVERY_OPTIONS = [
    { id: "ifood", label: "iFood (4%)", taxa: 0.04 },
    { id: "ubereats", label: "Uber Eats (6%)", taxa: 0.06 }
];

let carrinho = [];
let enderecoSelecionado = null;
let aplicativoSelecionado = APP_DELIVERY_OPTIONS[0];

function getUserOrders() {
    return getUserStorageItem("pedidos", []);
}

function saveUserOrders(pedidos) {
    setUserStorageItem("pedidos", pedidos);
}

function getUserAddresses() {
    return getUserStorageItem("enderecos", []);
}

function saveUserAddresses(enderecos) {
    setUserStorageItem("enderecos", enderecos);
}

function formatarPreco(valor) {
    return valor.toFixed(2).replace(".", ",");
}

function carregarCardapio() {
    const periodo = document.getElementById("periodo").value;
    const itens = MENU[periodo] || [];
    const container = document.getElementById("itens");
    container.innerHTML = "";

    itens.forEach(item => {
        container.innerHTML += `
            <div class="col-md-3">
                <div class="card food-card p-2 mb-4 shadow-sm">
                    <img src="https://source.unsplash.com/300x200/?food" />
                    <h5>${item.nome}</h5>
                    <p>R$ ${formatarPreco(item.preco)}</p>
                    <button class="btn btn-primary btn-sm" onclick='adicionarItem(${JSON.stringify(item)})'>
                        Adicionar
                    </button>
                </div>
            </div>
        `;
    });
}

function adicionarItem(item) {
    carrinho.push(item);
    atualizarCarrinho();
}

function calcularTaxaEntrega(subtotal) {
    const tipo = document.getElementById("tipo").value;
    if (tipo === "presencial") return 0;
    if (tipo === "delivery") return TAXA_DELIVERY_PROPRIO;
    if (tipo === "app") return subtotal * aplicativoSelecionado.taxa;
    return 0;
}

function atualizarCarrinho() {
    const div = document.getElementById("carrinho");
    const subtotal = carrinho.reduce((acc, item) => acc + item.preco, 0);
    const taxaEntrega = calcularTaxaEntrega(subtotal);
    const total = subtotal + taxaEntrega;
    div.innerHTML = "";

    if (carrinho.length === 0) {
        div.innerHTML = "<p class='text-muted'>Seu carrinho está vazio. Adicione pratos ao pedido.</p>";
    } else {
        carrinho.forEach(item => {
            div.innerHTML += `
                <div class="d-flex justify-content-between mb-2">
                    <span>${item.nome}</span>
                    <strong>R$ ${formatarPreco(item.preco)}</strong>
                </div>
            `;
        });
    }

    document.getElementById("subtotal-val").innerText = formatarPreco(subtotal);
    document.getElementById("taxa-entrega").innerText = formatarPreco(taxaEntrega);
    document.getElementById("total").innerText = formatarPreco(total);
}

function atualizarTipoEntrega() {
    const tipo = document.getElementById("tipo").value;
    document.getElementById("delivery-proprio-section").classList.toggle("d-none", tipo !== "delivery");
    document.getElementById("delivery-app-section").classList.toggle("d-none", tipo !== "app");
    document.getElementById("aviso-presencial").classList.toggle("d-none", tipo !== "presencial");
    atualizarCarrinho();
}

function atualizarAplicativo() {
    const valor = document.getElementById("app-comissao").value;
    aplicativoSelecionado = APP_DELIVERY_OPTIONS.find(option => option.id === valor) || APP_DELIVERY_OPTIONS[0];
    atualizarCarrinho();
}

function carregarEnderecos() {
    const enderecos = getUserAddresses();
    const lista = document.getElementById("enderecos-list");
    const select = document.getElementById("endereco-selecionado");
    lista.innerHTML = "";
    select.innerHTML = "";

    if (enderecos.length === 0) {
        lista.innerHTML = `
            <div class="alert alert-info">
                Você ainda não tem endereços cadastrados. Cadastre até 3 endereços para delivery próprio.
            </div>
        `;
        select.innerHTML = `<option value="">Nenhum endereço cadastrado</option>`;
        return;
    }

    enderecos.forEach(endereco => {
        const texto = `${endereco.rua}, ${endereco.numero} - ${endereco.cidade}/${endereco.estado} - CEP ${endereco.cep}`;
        const card = document.createElement("div");
        card.className = "border rounded p-3 mb-3";
        card.innerHTML = `
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <strong>${texto}</strong>
                <div class="text-muted">${endereco.complemento || "Sem complemento"}</div>
              </div>
              <button class="btn btn-outline-danger btn-sm" type="button" onclick="removerEndereco('${endereco.id}')">
                Excluir
              </button>
            </div>
        `;
        lista.appendChild(card);
        select.innerHTML += `<option value="${endereco.id}">${texto}</option>`;
    });

    if (!enderecoSelecionado && enderecos.length > 0) {
        enderecoSelecionado = enderecos[0].id;
    }
    select.value = enderecoSelecionado || "";
}

function adicionarEndereco(event) {
    event.preventDefault();
    const rua = document.getElementById("rua").value.trim();
    const numero = document.getElementById("numero").value.trim();
    const complemento = document.getElementById("complemento").value.trim();
    const cidade = document.getElementById("cidade").value.trim();
    const estado = document.getElementById("estado").value.trim();
    const cep = document.getElementById("cep").value.trim();

    if (!rua || !numero || !cidade || !estado || !cep) {
        alert("Preencha todos os campos do endereço.");
        return;
    }

    const enderecos = getUserAddresses();
    if (enderecos.length >= 3) {
        alert("Você já cadastrou o máximo de 3 endereços.");
        return;
    }

    enderecos.push({
        id: `${Date.now()}`,
        rua,
        numero,
        complemento,
        cidade,
        estado,
        cep
    });

    saveUserAddresses(enderecos);
    document.getElementById("enderecoForm").reset();
    carregarEnderecos();
}

function selecionarEndereco() {
    enderecoSelecionado = document.getElementById("endereco-selecionado").value;
}

function removerEndereco(id) {
    const enderecos = getUserAddresses().filter(endereco => endereco.id !== id);
    saveUserAddresses(enderecos);
    if (enderecoSelecionado === id) {
        enderecoSelecionado = null;
    }
    carregarEnderecos();
}

function carregarPedidos() {
    const pedidos = getUserOrders();
    const usuario = getCurrentUser();
    const div = document.getElementById("meusPedidos");
    div.innerHTML = "";

    if (!pedidos || pedidos.length === 0) {
        div.innerHTML = `
            <div class="alert alert-info">
                Ainda não há pedidos registrados. Adicione itens ao carrinho e finalize seu primeiro pedido.
            </div>
        `;
        return;
    }

    pedidos.slice().reverse().forEach(pedido => {
        div.innerHTML += `
            <div class="card p-3 mb-3">
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <div>
                        <h5 class="mb-1">Pedido de ${usuario.nome}</h5>
                        <small class="text-muted">${new Date(pedido.data).toLocaleString("pt-BR")}</small>
                    </div>
                    <span class="badge bg-secondary">${pedido.tipoAtendimento}</span>
                </div>
                <div class="mb-3">
                    ${pedido.itens.map(item => `
                        <div class="d-flex justify-content-between mb-1">
                            <span>${item.nome}</span>
                            <strong>R$ ${formatarPreco(item.preco)}</strong>
                        </div>
                    `).join("")}
                </div>
                <hr>
                <div class="d-flex justify-content-between">
                    <span>Subtotal</span>
                    <strong>R$ ${formatarPreco(pedido.subtotal)}</strong>
                </div>
                <div class="d-flex justify-content-between">
                    <span>Taxa de entrega</span>
                    <strong>R$ ${formatarPreco(pedido.taxaEntrega)}</strong>
                </div>
                <div class="d-flex justify-content-between fw-bold mt-2">
                    <span>Total</span>
                    <strong>R$ ${formatarPreco(pedido.total)}</strong>
                </div>
                ${pedido.tipoAtendimento === "Delivery Próprio" ? `
                    <div class="mt-3 text-muted">Endereço: ${pedido.endereco}</div>
                ` : ""}
                ${pedido.tipoAtendimento === "Delivery Aplicativo" ? `
                    <div class="mt-3 text-muted">Aplicativo: ${pedido.appNome} (${pedido.appTaxa * 100}%)</div>
                ` : ""}
            </div>
        `;
    });
}

function finalizarPedido() {
    if (carrinho.length === 0) {
        alert("Adicione ao menos um item ao carrinho antes de finalizar o pedido.");
        return;
    }

    const tipo = document.getElementById("tipo").value;
    const usuario = getCurrentUser();
    if (!usuario) {
        window.location.href = "login.html";
        return;
    }

    if (tipo === "presencial") {
        const reservas = getUserStorageItem("reservas", []);
        if (!reservas || reservas.length === 0) {
            alert("Para atendimento presencial, você precisa fazer uma reserva em Minhas Reservas antes de finalizar o pedido.");
            return;
        }
    }

    if (tipo === "delivery" && !enderecoSelecionado) {
        alert("Selecione um endereço para entrega própria antes de finalizar o pedido.");
        return;
    }

    if (tipo === "app" && !aplicativoSelecionado) {
        alert("Selecione uma opção de delivery por aplicativo antes de finalizar o pedido.");
        return;
    }

    const subtotal = carrinho.reduce((acc, item) => acc + item.preco, 0);
    const taxaEntrega = calcularTaxaEntrega(subtotal);
    const total = subtotal + taxaEntrega;
    const pedidos = getUserOrders();

    const enderecoSelecionadoObj = tipo === "delivery" ? getUserAddresses().find(e => e.id === enderecoSelecionado) : null;
    const enderecoTexto = enderecoSelecionadoObj ? `${enderecoSelecionadoObj.rua}, ${enderecoSelecionadoObj.numero} - ${enderecoSelecionadoObj.cidade}/${enderecoSelecionadoObj.estado}` + (enderecoSelecionadoObj.complemento ? `, ${enderecoSelecionadoObj.complemento}` : "") + ` - CEP ${enderecoSelecionadoObj.cep}` : null;

    const pedido = {
        id: Date.now(),
        data: new Date().toISOString(),
        clienteNome: usuario.nome,
        tipoAtendimento: tipo === "delivery" ? "Delivery Próprio" : tipo === "app" ? "Delivery Aplicativo" : "Presencial",
        endereco: tipo === "delivery" ? enderecoTexto : null,
        appNome: tipo === "app" ? aplicativoSelecionado.label : null,
        appTaxa: tipo === "app" ? aplicativoSelecionado.taxa : 0,
        itens: carrinho.map(item => ({ nome: item.nome, preco: item.preco })),
        subtotal,
        taxaEntrega,
        total
    };

    pedidos.push(pedido);
    saveUserOrders(pedidos);
    carrinho = [];
    atualizarCarrinho();
    carregarPedidos();
    alert("Pedido finalizado com sucesso!");
}

function inicializarPagina() {
    const usuario = requireLogin();
    if (!usuario) return;

    document.getElementById("usuario-nome").innerText = usuario.nome;
    document.getElementById("btn-adicionar-endereco").addEventListener("click", adicionarEndereco);
    document.getElementById("endereco-selecionado").addEventListener("change", selecionarEndereco);
    document.getElementById("tipo").addEventListener("change", atualizarTipoEntrega);
    document.getElementById("app-comissao").addEventListener("change", atualizarAplicativo);

    carregarCardapio();
    carregarEnderecos();
    atualizarTipoEntrega();
    atualizarCarrinho();
    carregarPedidos();
}

window.addEventListener("DOMContentLoaded", inicializarPagina);
