const API_URL = "https://localhost:7189";

async function getData(endpoint) {
    const response = await fetch(API_URL + endpoint, {
        credentials: "include"
    });
    return response.json();
}

async function postData(endpoint, data) {
    const response = await fetch(API_URL + endpoint, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        credentials: "include",
        body: JSON.stringify(data)
    });
    return response.json();
}