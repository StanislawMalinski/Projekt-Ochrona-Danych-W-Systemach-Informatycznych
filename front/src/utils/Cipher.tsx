import { sha256, sha384} from "crypto-hash";
import  secureLocalStorage  from  "react-secure-storage";
import config from "../../clientconfig.json"
import { useState } from "react";

const iv_ = "0535627058893800";

async function hash(password: string) {
    for (let i = 0; i < config.key.iterations; i++) {
        password = await sha256(password);   
    }
    return password;
}

function saveCredentials(email: string, aesKey: string) {
    secureLocalStorage.setItem("email", email);
    secureLocalStorage.setItem("aesKey", hash(aesKey));
}

function getCredentials() {
    return {
        email: secureLocalStorage.getItem("email"),
        password: secureLocalStorage.getItem("aesKey")
    }
}

function deleteCredentials() {
    secureLocalStorage.removeItem("email");
    secureLocalStorage.removeItem("aesKey");
}

function saveServerPubKey(key: string) {
    secureLocalStorage.setItem("serverKey", key);
}

function encryptWithServerPubKey(data: string) {
    var key = secureLocalStorage.getItem("serverKey");
    if (key) {
        return null;
    } else {
        return null;
    }
}


export { 
    hash, 
    saveCredentials,
    getCredentials, 
    deleteCredentials, 
    saveServerPubKey, 
    encryptWithServerPubKey
    };