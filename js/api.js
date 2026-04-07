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

function getCurrentUser() {
    const current = localStorage.getItem("restaurante_usuario_atual");
    return current ? JSON.parse(current) : null;
}

function setCurrentUser(user) {
    localStorage.setItem("restaurante_usuario_atual", JSON.stringify(user));
}

function clearCurrentUser() {
    localStorage.removeItem("restaurante_usuario_atual");
}

function requireLogin(redirectPage = "login.html") {
    const user = getCurrentUser();
    if (!user) {
        window.location.href = redirectPage;
        return null;
    }
    return user;
}

function getUserStorageKey(baseKey) {
    const user = getCurrentUser();
    return user ? `${baseKey}_${user.email}` : baseKey;
}

function getUserStorageItem(key, fallback = null) {
    const storageKey = getUserStorageKey(key);
    const value = localStorage.getItem(storageKey);
    return value ? JSON.parse(value) : fallback;
}

function setUserStorageItem(key, value) {
    const storageKey = getUserStorageKey(key);
    localStorage.setItem(storageKey, JSON.stringify(value));
}
