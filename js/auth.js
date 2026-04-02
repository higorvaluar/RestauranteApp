document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const data = {
        email: document.getElementById("email").value,
        senha: document.getElementById("senha").value
    };

    await postData("/Auth/Login", data);

    window.location.href = "index.html";
});