async function carregarHome() {
    const data = await getData("/");

    console.log(data);

    // você vai renderizar aqui depois
}

carregarHome();