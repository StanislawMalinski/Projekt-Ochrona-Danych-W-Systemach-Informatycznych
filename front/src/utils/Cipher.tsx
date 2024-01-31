import { sha256 } from "crypto-hash";
import  secureLocalStorage  from  "react-secure-storage";
import config from "../../clientconfig.json"

async function hash(password: string) {
    for (let i = 0; i < config.key.iterations; i++) {
        password = await sha256(password);   
    }
    return password;
}

async function hashPassword(password: string) {
    hash(password).then((key) => {
        return key.slice(0, 32);
    });
}

async function saveCredentials(email: string,  token: string) {
    secureLocalStorage.setItem("email", email);
    secureLocalStorage.setItem("token", token); 
}

function getCredentials() {
    return {
        email: secureLocalStorage.getItem("email"),
    }
}

function deleteCredentials() {
    secureLocalStorage.removeItem("email");
    secureLocalStorage.removeItem("token");
}

function getToken(): any {
    return JSON.parse(secureLocalStorage.getItem("token") as string);
}

export { 
    hash, 
    saveCredentials,
    getCredentials, 
    deleteCredentials, 
    getToken,
    hashPassword
    };