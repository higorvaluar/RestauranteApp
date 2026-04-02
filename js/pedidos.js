let carrinho = [];

async function carregarCardapio() {
    const periodo = document.getElementById("periodo").value;

    const data = await getData("/");

    const itens = periodo === "almoco" ? data.almoco : data.jantar;

    const container = document.getElementById("itens");
    container.innerHTML = "";

    itens.forEach(item => {
        container.innerHTML += `
            <div class="col-md-3">
                <div class="card food-card p-2 mb-4 shadow-sm">
                    <img src="https://source.unsplash.com/300x200/?food" />
                    <h5>${item.nome}</h5>
                    <p>R$ ${item.preco}</p>
                    <button class="btn btn-primary btn-sm"
                        onclick='adicionarItem(${JSON.stringify(item)})'>
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

function atualizarCarrinho() {
    const div = document.getElementById("carrinho");
    div.innerHTML = "";

    let total = 0;

    carrinho.forEach(item => {
        total += item.preco;

        div.innerHTML += `
            <p>${item.nome} - R$ ${item.preco}</p>
        `;
    });

    document.getElementById("total").innerText = total.toFixed(2);
}

async function finalizarPedido() {
    const periodo = document.getElementById("periodo").value;
    const tipo = document.getElementById("tipo").value;

    const data = {
        periodo,
        tipo,
        itens: carrinho
    };

    await postData("/Pedidos/Create", data);

    alert("Pedido realizado!");

    carrinho = [];
    atualizarCarrinho();
    carregarPedidos();
}

async function carregarPedidos() {
    const pedidos = await getData("/Pedidos");

    const div = document.getElementById("meusPedidos");
    div.innerHTML = "";

    pedidos.forEach(p => {
        div.innerHTML += `
            <div class="card p-3 mb-2">
                <p><strong>Data:</strong> ${p.data}</p>
                <p><strong>Total:</strong> R$ ${p.total}</p>
                <a href="/Pedidos/Details/${p.id}" class="btn btn-sm btn-outline-dark">
                    Ver detalhes
                </a>
            </div>
        `;
    });
}

/* EVENTOS */
document.getElementById("periodo").addEventListener("change", carregarCardapio);

/* INIT */
carregarCardapio();
carregarPedidos();