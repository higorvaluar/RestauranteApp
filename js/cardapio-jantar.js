const jantar = [
  { nome: "Risoto de Camarão", preco: 64.90, descricao: "Cremoso e sofisticado.", imagem: "https://source.unsplash.com/300x200/?shrimp-risotto" },
  { nome: "Risoto de Funghi", preco: 49.90, descricao: "Cogumelos selecionados.", imagem: "https://source.unsplash.com/300x200/?mushroom-risotto" },
  { nome: "Salmão Grelhado", preco: 59.90, descricao: "Com ervas.", imagem: "https://source.unsplash.com/300x200/?salmon" },
  { nome: "Filé Mignon", preco: 54.90, descricao: "Corte nobre.", imagem: "https://source.unsplash.com/300x200/?steak" },
  { nome: "Nhoque ao Queijo", preco: 44.90, descricao: "Leve e cremoso.", imagem: "https://source.unsplash.com/300x200/?gnocchi" },
  { nome: "Espaguete ao Pesto", preco: 39.90, descricao: "Aromático.", imagem: "https://source.unsplash.com/300x200/?pasta" },
  { nome: "Frango Mostarda e Mel", preco: 42.90, descricao: "Equilíbrio perfeito.", imagem: "https://source.unsplash.com/300x200/?chicken" },
  { nome: "Costela Assada", preco: 52.90, descricao: "Muito macia.", imagem: "https://source.unsplash.com/300x200/?ribs" },
  { nome: "Tilápia Crocante", preco: 46.90, descricao: "Leve e sofisticada.", imagem: "https://source.unsplash.com/300x200/?tilapia" },
  { nome: "Pizza Artesanal", preco: 38.90, descricao: "Ingredientes frescos.", imagem: "https://source.unsplash.com/300x200/?pizza" },
  { nome: "Lasanha de Frango", preco: 41.90, descricao: "Molho cremoso.", imagem: "https://source.unsplash.com/300x200/?lasagna" },
  { nome: "Medalhão ao Molho Madeira", preco: 58.90, descricao: "Elegante.", imagem: "https://source.unsplash.com/300x200/?steak" },
  { nome: "Ravioli", preco: 45.90, descricao: "Recheio especial.", imagem: "https://source.unsplash.com/300x200/?ravioli" },
  { nome: "Polenta com Carne", preco: 40.90, descricao: "Cremosa.", imagem: "https://source.unsplash.com/300x200/?polenta" },
  { nome: "Frango ao Curry", preco: 43.90, descricao: "Aromático.", imagem: "https://source.unsplash.com/300x200/?curry" },
  { nome: "Paella", preco: 69.90, descricao: "Espanhola.", imagem: "https://source.unsplash.com/300x200/?paella" },
  { nome: "Bacalhau ao Forno", preco: 62.90, descricao: "Clássico.", imagem: "https://source.unsplash.com/300x200/?codfish" },
  { nome: "Carpaccio", preco: 39.90, descricao: "Entrada leve.", imagem: "https://source.unsplash.com/300x200/?carpaccio" },
  { nome: "Hambúrguer Gourmet", preco: 34.90, descricao: "Artesanal.", imagem: "https://source.unsplash.com/300x200/?burger" },
  { nome: "Camarão Empanado", preco: 57.90, descricao: "Crocante.", imagem: "https://source.unsplash.com/300x200/?shrimp" }
];

// Prato do dia para o jantar
const pratoDoDiaJantar = {
  nome: "Salmão Grelhado com Legumes ao Azeite",
  preco: 59.90,
  descricao: "Salmão fresco grelhado com ervas, acompanhado de legumes refogados ao azeite de oliva extra virgem.",
  imagem: "https://source.unsplash.com/400x300/?salmon-grill"
};

// Sugestão do chef para jantar
const sugestaoChefJantar = {
  nome: "Filé Mignon ao Molho de Vinho com Trufa",
  preco: 54.90,
  descricao: "Filé mignon grelhado ao ponto, acompanhado de um refinado molho madeira com toque de trufa. Uma experiência gastronômica única.",
  imagem: "https://source.unsplash.com/400x300/?filet-mignon"
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
      <img class="chef-img" src="${pratoDoDiaJantar.imagem}"/>
      <div class="chef-content">
        <span class="chef-badge">⭐ Prato do Dia</span>
        <div class="chef-title">${pratoDoDiaJantar.nome}</div>
        <p>${pratoDoDiaJantar.descricao}</p>
        <div class="chef-price">R$ ${pratoDoDiaJantar.preco}</div>
      </div>
    </div>
  `;
}

function renderizarSugestao() {
  const div = document.getElementById("sugestao");

  div.innerHTML = `
    <div class="chef-card">
      <img class="chef-img" src="${sugestaoChefJantar.imagem}"/>
      <div class="chef-content">
        <span class="chef-badge">👨‍🍳 Recomendação do Chef</span>
        <div class="chef-title">${sugestaoChefJantar.nome}</div>
        <p>${sugestaoChefJantar.descricao}</p>
        <div class="chef-price">R$ ${sugestaoChefJantar.preco}</div>
      </div>
    </div>
  `;
}

renderizar("jantar", jantar);
renderizarPratoDoDia();
renderizarSugestao();
