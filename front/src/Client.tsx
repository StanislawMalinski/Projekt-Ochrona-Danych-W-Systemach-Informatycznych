import config from '../clientconfig.json';
import {getToken} from './utils/Cipher';

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
    });
}


function logincodesubmit(body: any) {
    return fetch(baseUrl + config.urls.logincodesubmit, {
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
    });
}


function logout() {
    var body = {token: getToken()};
    return fetch(baseUrl + config.urls.logout, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin,
            'Authorization': body.token
        },
        body: JSON.stringify(body)
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
    });
}

function submitregistrationcode(body: any) {
    body.token = getToken();
    return fetch(baseUrl + config.urls.submitregistrationcode, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => { return response.json(); });
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
    });
}

function passwordchange(body: any) {
    console.log(body);
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
    });
}

function transfer(body: any) {
    body.token = getToken();
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
    });
}

function account() {
    var body = {token: getToken()};
    return fetch(baseUrl + config.urls.account, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {;
        if (response.status === 200) {
            return response.json();
        }
    });
}

function getactivities(email: string) {
    const body: { token: string } = getToken();
    return fetch(baseUrl + config.urls.activities + email, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': accessControlAllowOrigin
        },
        body: JSON.stringify(body)
    }).then((response) => {
        if (response.status === 200) {;
            return response.json();
        }
    });
}

export {login,
        logincodesubmit,
        logout,
        register,
        submitregistrationcode,
        passwordchangerequestcode, 
        codesubmit,
        passwordchange,
        transfer, 
        account, 
        getactivities}


