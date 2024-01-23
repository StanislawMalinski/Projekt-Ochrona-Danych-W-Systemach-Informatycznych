import { useEffect } from 'react';
import config from '../clientconfig.json';
import { saveServerPubKey } from './utils/Cipher';

var baseUrl = '';
var accessControlAllowOrigin = '';
baseUrl = config.production.server.protocol + '://' 
    + config.production.server.host + ':' 
    + config.production.server.port
    + config.baseUrl;
accessControlAllowOrigin = config.production.host.protocol + '://'
    + config.production.host.host + ':'
    + config.production.host.port;

// require password
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

// require password
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

function submitregistrationcode(body: any) {
    return fetch(baseUrl + config.urls.submitregistrationcode, {
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

function passwordchangerequestcode(body: any) {
    return fetch(baseUrl + config.urls.passwordchangerequestcode, {
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

function codesubmit(body: any) {
    return fetch(baseUrl + config.urls.codesubmit, {
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

// require password
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

function getactivities(email: string) {
    return fetch(baseUrl + config.urls.activities + email, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        }
    }).then((response) => {
        if (response.status === 200) {
            console.log(response);
            return response.json();
        }
    }).catch((error) => {
        console.log(error);
    });
}

export {login,
        register,
        submitregistrationcode,
        passwordchangerequestcode, 
        codesubmit,
        passwordchange,
        transfer, 
        account, 
        getPubKey,
        getactivities}