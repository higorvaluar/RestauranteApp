clearCurrentUser();

const usuarioAtual = getCurrentUser();
if (usuarioAtual) {
    window.location.href = "pedidos.html";
}

document.getElementById("loginForm").addEventListener("submit", (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value.trim();
    const senha = document.getElementById("senha").value;
    const contas = JSON.parse(localStorage.getItem("restaurante_usuarios") || "[]");

    const usuario = contas.find(u => u.email.toLowerCase() === email.toLowerCase() && u.senha === senha);

    if (!usuario) {
        alert("E-mail ou senha inválidos. Cadastre-se se ainda não tem conta.");
        return;
    }

    setCurrentUser({
        nome: usuario.nome,
        email: usuario.email,
        telefone: usuario.telefone
    });

    window.location.href = "pedidos.html";
});
