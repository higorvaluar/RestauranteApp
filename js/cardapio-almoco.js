const almoco = [
  { nome: "Filé de Frango Grelhado", preco: 24.90, descricao: "Acompanhado de arroz, feijão e salada fresca.", imagem: "https://source.unsplash.com/300x200/?grilled-chicken" },
  { nome: "Bife Acebolado", preco: 29.90, descricao: "Carne macia com cebolas caramelizadas.", imagem: "https://source.unsplash.com/300x200/?beef-steak" },
  { nome: "Frango à Parmegiana", preco: 32.90, descricao: "Empanado com molho e queijo gratinado.", imagem: "https://source.unsplash.com/300x200/?chicken-parmesan" },
  { nome: "Carne à Parmegiana", preco: 36.90, descricao: "Clássico italiano com molho artesanal.", imagem: "https://source.unsplash.com/300x200/?beef-parmesan" },
  { nome: "Strogonoff de Frango", preco: 28.90, descricao: "Cremoso com arroz e batata palha.", imagem: "https://source.unsplash.com/300x200/?stroganoff" },
  { nome: "Strogonoff de Carne", preco: 31.90, descricao: "Molho rico e carne suculenta.", imagem: "https://source.unsplash.com/300x200/?beef-stroganoff" },
  { nome: "Peixe Grelhado", preco: 34.90, descricao: "Leve e saudável com legumes.", imagem: "https://source.unsplash.com/300x200/?grilled-fish" },
  { nome: "Lasanha à Bolonhesa", preco: 30.90, descricao: "Camadas de carne e queijo.", imagem: "https://source.unsplash.com/300x200/?lasagna" },
  { nome: "Macarrão ao Molho Branco", preco: 27.90, descricao: "Cremoso e bem temperado.", imagem: "https://source.unsplash.com/300x200/?pasta" },
  { nome: "Feijoada Completa", preco: 35.90, descricao: "Tradicional com farofa e couve.", imagem: "https://source.unsplash.com/300x200/?feijoada" },
  { nome: "Frango Assado", preco: 26.90, descricao: "Temperado e suculento.", imagem: "https://source.unsplash.com/300x200/?roasted-chicken" },
  { nome: "Costela com Mandioca", preco: 39.90, descricao: "Carne macia com mandioca.", imagem: "https://source.unsplash.com/300x200/?beef-ribs" },
  { nome: "Arroz Carreteiro", preco: 28.90, descricao: "Tradicional e saboroso.", imagem: "https://source.unsplash.com/300x200/?rice-meat" },
  { nome: "Tilápia Frita", preco: 33.90, descricao: "Crocante por fora.", imagem: "https://source.unsplash.com/300x200/?fried-fish" },
  { nome: "Omelete Completo", preco: 22.90, descricao: "Com queijo e presunto.", imagem: "https://source.unsplash.com/300x200/?omelette" },
  { nome: "Panqueca de Carne", preco: 25.90, descricao: "Molho caseiro delicioso.", imagem: "https://source.unsplash.com/300x200/?pancake" },
  { nome: "Arroz com Frango", preco: 26.90, descricao: "Simples e saboroso.", imagem: "https://source.unsplash.com/300x200/?chicken-rice" },
  { nome: "Carne de Panela", preco: 32.90, descricao: "Molho encorpado.", imagem: "https://source.unsplash.com/300x200/?beef-stew" },
  { nome: "Frango com Quiabo", preco: 29.90, descricao: "Clássico brasileiro.", imagem: "https://source.unsplash.com/300x200/?chicken" },
  { nome: "Moqueca de Peixe", preco: 37.90, descricao: "Rica em sabores.", imagem: "https://source.unsplash.com/300x200/?fish-stew" }
];

// Prato do dia para o almoço
const pratoDoDiaAlmoco = {
  nome: "Filé de Frango Grelhado com Risoto de Abóbora",
  preco: 24.90,
  descricao: "Frango suculento acompanhado de um risoto cremoso de abóbora, com arroz e salada fresca.",
  imagem: "https://source.unsplash.com/400x300/?grilled-chicken-rice"
};

// Sugestão do chef para almoço
const sugestaoChefAlmoco = {
  nome: "Costela com Mandioca ao Molho de Vinho",
  preco: 39.90,
  descricao: "Costela macia com mandioca, acompanhada de um molho de vinho tinto encorpado. Uma combinação clássica e sofisticada.",
  imagem: "https://source.unsplash.com/400x300/?beef-ribs"
};

function renderizar(containerId, itens) {
  const div = document.getElementById(containerId);
  div.innerHTML = "";

  itens.forEach(item => {
    div.innerHTML += `
      <div class="col-md-3">
        <div class="card food-card p-2 mb-4 shadow-sm">
          <img src="${item.imagem}" />
          <div class="p-2">
            <h5>${item.nome}</h5>
            <p class="text-muted small">${item.descricao}</p>
            <p class="fw-bold text-danger">R$ ${item.preco}</p>
            <button class="btn btn-primary w-100">Adicionar</button>
          </div>
        </div>
      </div>
    `;
  });
}

function renderizarPratoDoDia() {
  const div = document.getElementById("prato-do-dia");

  div.innerHTML = `
    <div class="chef-card">
      <img class="chef-img" src="${pratoDoDiaAlmoco.imagem}"/>
      <div class="chef-content">
        <span class="chef-badge">⭐ Prato do Dia</span>
        <div class="chef-title">${pratoDoDiaAlmoco.nome}</div>
        <p>${pratoDoDiaAlmoco.descricao}</p>
        <div class="chef-price">R$ ${pratoDoDiaAlmoco.preco}</div>
      </div>
    </div>
  `;
}

function renderizarSugestao() {
  const div = document.getElementById("sugestao");

  div.innerHTML = `
    <div class="chef-card">
      <img class="chef-img" src="${sugestaoChefAlmoco.imagem}"/>
      <div class="chef-content">
        <span class="chef-badge">👨‍🍳 Recomendação do Chef</span>
        <div class="chef-title">${sugestaoChefAlmoco.nome}</div>
        <p>${sugestaoChefAlmoco.descricao}</p>
        <div class="chef-price">R$ ${sugestaoChefAlmoco.preco}</div>
      </div>
    </div>
  `;
}

renderizar("almoco", almoco);
renderizarPratoDoDia();
renderizarSugestao();
