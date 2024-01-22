import config from '../clientconfig.json';
import {sha256} from 'crypto-hash';
import { saveServerPubKey } from './utils/Cipher';

var baseUrl = '';
var accessControlAllowOrigin = '';
if (process.env.NODE_ENV === 'development') {
    baseUrl = config.development.server.protocol + '://' 
        + config.development.server.host + ':' 
        + config.development.server.port
        + config.baseUrl;
    accessControlAllowOrigin += config.development.host.protocol + '://'
        + config.development.host.host + ':'
        + config.development.host.port;
    
} else if (process.env.NODE_ENV === 'production') {
    baseUrl = config.production.server.protocol + '://' 
        + config.production.server.host + ':' 
        + config.production.server.port
        + config.baseUrl;
    accessControlAllowOrigin = config.production.host.protocol + '://'
        + config.production.host.host + ':'
        + config.production.host.port;
}

function login(body: any) {
    return fetch(baseUrl + config.urls.login, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {
        if (response.status === 200) {
            return response.json();
        }
    }).catch((error) => {
        console.log(error);
    });

}

function register(body: any) {
    return fetch(baseUrl + config.urls.register, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {
        if (response.status === 200) {
            return response.json();
        }
    }).catch((error) => {
        console.log(error);
    });
}

function passwordchange(body: any) {
    return fetch(baseUrl + config.urls.passwordchange, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {
        if (response.status === 200) {
            return response.json();
        }
    }).catch((error) => {
        console.log(error);
    });
}

function transfer(body: any) {
    return fetch(baseUrl + config.urls.transfer, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {
        if (response.status === 200) {
            return response.json();
        }
    }).catch((error) => {
        console.log(error);
    });
}

function account(body: any) {
    return fetch(baseUrl + config.urls.account, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {
        console.log(response);
        if (response.status === 200) {
            return response.json();
        }
    }).catch((error) => {
        console.log(error);
    });
}

function getPubKey() {
    return fetch(baseUrl + config.urls.pubkey, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        }
    }).then((response) => {
        if (response.status === 200) {
            console.log(response);
            response.json().then((data) => {
                console.log(data);
                saveServerPubKey(data);
            });
        }
    }).catch((error) => {
        console.log(error);
    });
}

export {login, register, passwordchange, transfer, account, getPubKey}