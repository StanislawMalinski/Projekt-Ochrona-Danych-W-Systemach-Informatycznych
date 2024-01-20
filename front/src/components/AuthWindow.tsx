import { useState } from 'react';
import '../styles/AuthWindow.css'
import PassComp from './password/PassComp';
import LoginComp from './auth/LoginComp';
import RegisterComp from './auth/RegisterComp';
import { encryptWithAesKey, decryptWithAesKey, saveCredentials, getCredentials } from '../utils/Cipher';

import {login, register} from '../Client';


interface AuthWindowProps {
    logged: boolean;
    setLogged: (arg: boolean) => void;
}

function AuthWindow(props: AuthWindowProps) {
    const {logged, setLogged} = props;
    const [hasAc, setHasAc] = useState(true);

    const [registerRequest, setRegisterRequest] = useState({email: "", password: ""})
    const [loginRequest, setLoginRequest] = useState({email: "", password: ""})

    const [disableRegistery, setDisableRegistery] = useState(true)

    const auth = () => {
        if (hasAc) {
            login(loginRequest)
            .then((response) => {
                if (response) {
                    saveCredentials(loginRequest.email, loginRequest.password)
                    setLogged(true)
                } else {
                    console.log("Error in login")
                }
            });
        } else {
            register(registerRequest)
        }
    }

    var content = hasAc ? "Login" : "Register"
    var goTo = hasAc ? "Register" : "Login"

    return (<> {logged ?
        <></>
        :
        <div className='auth-window'>
            <div className='auth-window-inner'>
                <h1>{content}</h1>
                {hasAc ? 
                    <LoginComp setLoginRequest={setLoginRequest}/> 
                    : 
                    <RegisterComp setRegisterRequest={setRegisterRequest} setDisableRegistery={setDisableRegistery}/>
                }
                <button onClick={() => setHasAc(!hasAc)}>{goTo}</button>
                <button onClick={() => auth()} disabled={disableRegistery} >{content}</button>


                <button onClick={() => setLogged(!logged)}>"Login"</button>
            </div>
        </div>
        }
    </>)
}

export default AuthWindow;