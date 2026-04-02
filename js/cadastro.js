document.getElementById("cadastroForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const data = {
        nome: document.getElementById("nome").value,
        email: document.getElementById("email").value,
        senha: document.getElementById("senha").value,
        telefone: document.getElementById("telefone").value
    };

    try {
        await postData("/Auth/Register", data);

        alert("Cadastro realizado com sucesso!");

        window.location.href = "login.html";

    } catch (error) {
        alert("Erro ao cadastrar!");
    }
});