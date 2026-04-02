async function reservar() {
    const data = {
        data: document.getElementById("data").value,
        pessoas: document.getElementById("pessoas").value
    };

    await postData("/Reservas/Create", data);

    alert("Reserva feita!");
}