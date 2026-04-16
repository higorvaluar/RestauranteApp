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

function renderizarSugestao() {
  const div = document.getElementById("sugestao");

  div.innerHTML = `
    <div class="chef-card">
      <img class="chef-img" src="https://source.unsplash.com/400x300/?gourmet-food"/>
      <div class="chef-content">
        <span class="chef-badge">🔥 Mais pedido do dia</span>
        <div class="chef-title">Filé Mignon ao Molho de Vinho</div>
        <p>Altamente avaliado pelos clientes pela maciez e sabor sofisticado.</p>
        <div class="chef-price">R$ 59,90</div>
      </div>
    </div>
  `;
}

renderizar("almoco", almoco);
renderizar("jantar", jantar);
renderizarSugestao();