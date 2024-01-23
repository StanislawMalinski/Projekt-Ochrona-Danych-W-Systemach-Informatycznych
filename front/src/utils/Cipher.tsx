import { sha256 } from "crypto-hash";
import  secureLocalStorage  from  "react-secure-storage";
import config from "../../clientconfig.json"
import CryptoJS from "crypto-js";
const iv_ = "0535627058893800";

async function hash(password: string) {
    for (let i = 0; i < config.key.iterations; i++) {
        password = await sha256(password);   
    }
    return password;
}

async function getAesKey(password: string) {
    hash(password).then((key) => {
        return key.slice(0, 32);
    });
}

function saveCredentials(email: string, password: string) {
    secureLocalStorage.setItem("email", email);
    secureLocalStorage.setItem("aesKey", getAesKey(password));
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

function aes(data: string, key: string, mode: string) {
    let plaintext = CryptoJS.enc.Utf8.parse(data);
    let secSpec = CryptoJS.enc.Utf8.parse(key);    
    let ivSpec = CryptoJS.enc.Utf8.parse(iv_);

    secSpec = CryptoJS.lib.WordArray.create(secSpec.words.slice(0, 16/4));
    ivSpec = CryptoJS.lib.WordArray.create(ivSpec.words.slice(0, 16/4));

    if (mode === "enc") {
        var encrypted = CryptoJS.AES.encrypt(plaintext, secSpec, {iv: ivSpec});
        return encrypted.toString();
    } else if (mode === "dec") {
        var decrypted = CryptoJS.AES.decrypt(data, secSpec, {iv: ivSpec});
        return decrypted.toString(CryptoJS.enc.Utf8);
    }
}

function encryptWithServerPubKey(data: string) {
    var key = secureLocalStorage.getItem("serverKey");
    if (key) {
        return data;
    }
}

export { 
    hash, 
    saveCredentials,
    getCredentials, 
    deleteCredentials, 
    saveServerPubKey, 
    encryptWithServerPubKey,
    aes
    };