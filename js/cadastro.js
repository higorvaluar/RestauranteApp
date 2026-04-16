const usuarioAtualCadastro = getCurrentUser();
if (usuarioAtualCadastro) {
    window.location.href = "pedidos.html";
}

document.getElementById("cadastroForm").addEventListener("submit", (e) => {
    e.preventDefault();

    const nome = document.getElementById("nome").value.trim();
    const email = document.getElementById("email").value.trim();
    const senha = document.getElementById("senha").value;
    const confirmarSenha = document.getElementById("confirmarSenha").value;
    const telefone = document.getElementById("telefone").value.trim();

    if (senha !== confirmarSenha) {
        alert("As senhas não coincidem.");
        return;
    }

    const contas = JSON.parse(localStorage.getItem("restaurante_usuarios") || "[]");
    const emailJaCadastrado = contas.some(u => u.email.toLowerCase() === email.toLowerCase());

    if (emailJaCadastrado) {
        alert("Este e-mail já está cadastrado. Faça login ou use outro e-mail.");
        return;
    }

    contas.push({
        nome,
        email,
        senha,
        telefone
    });

    localStorage.setItem("restaurante_usuarios", JSON.stringify(contas));
    alert("Cadastro realizado com sucesso!");
    window.location.href = "login.html";
});
