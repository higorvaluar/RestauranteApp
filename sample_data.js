// Dados de teste para desenvolvimento
const usuariosTeste = [
    {
        nome: "João Silva",
        email: "joao@email.com",
        senha: "123456",
        telefone: "11999999999"
    },
    {
        nome: "Maria Santos",
        email: "maria@email.com",
        senha: "123456",
        telefone: "11888888888"
    },
    {
        nome: "Gabriela Dorneles",
        email: "gabi@email.com",
        senha: "gabi",
        telefone: "11777777777"
    }
];

// Salvar no localStorage se não existir
if (!localStorage.getItem("restaurante_usuarios")) {
    localStorage.setItem("restaurante_usuarios", JSON.stringify(usuariosTeste));
}

console.log("Dados de teste carregados. Usuários disponíveis:");
usuariosTeste.forEach(u => console.log(`${u.nome}: ${u.email} / ${u.senha}`));