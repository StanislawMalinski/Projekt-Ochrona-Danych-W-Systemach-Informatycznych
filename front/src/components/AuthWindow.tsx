import { useEffect, useState } from 'react';
import '../styles/AuthWindow.css'
import LoginComp from './auth/LoginComp';
import RegisterComp from './auth/RegisterComp';

import {login, register} from '../Client';
import { saveCredentials } from '../utils/Cipher';


interface AuthWindowProps {
    logged: boolean;
    setLogged: (arg: boolean) => void;
    setAccount: (arg: {accountNumber: string, balance: number, history: {accountNumber: string, recipientAccountNumber: string, recipient: string, title: string, value: number}[]}) => void;
}

function AuthWindow(props: AuthWindowProps) {
    const {logged, setLogged, setAccount} = props;
    const [mode, setMode] = useState("login");
    const [content, setContent] = useState(<></>);

    const [registerRequest, setRegisterRequest] = useState({email: "", password: ""})
    const [loginRequest, setLoginRequest] = useState({email: "", password: ""})

    const [message, setMessage] = useState("");
    const [disabled, setDisabled] = useState(false);

    useEffect(() => {
        setMessage("");
    }, [mode, logged]);

    const auth = (m: string ) => {
        console.log(m)
        switch (m) {
            case "login":
                login(loginRequest)
                .then((response) => {
                    if (response.success) {
                        saveCredentials(loginRequest.email, loginRequest.password)
                        setLogged(true);
                        setAccount(response);
                    } else {
                        setMessage(response.message);
                    }
                });
                break;
            case "register":
                register(registerRequest)
                .then((response) => {
                    if (response.success) {
                        saveCredentials(registerRequest.email, registerRequest.password)
                        setLogged(true);
                        setAccount(response);
                    } else {
                        setMessage(response.message);
                    }
                });
            break;
            case "password":
                break;
        }
    }

    var reg = (<>
        <RegisterComp setRegisterRequest={setRegisterRequest} setMessage={(e) => setMessage(e)} setDisabled={(e) => setDisabled(e)}/>
        <p className='login-option' onClick={() => setMode("login")}>You have an account?</p>
        </>)
    var log = (<>
        <LoginComp setLoginRequest={setLoginRequest}/>
        <p className='login-option' onClick={() => setMode("password")}>Forgot password?</p>
        <p className={'login-option ' } onClick={() => setMode("register")} >Sign in</p>
        </>);
    var pass = (<>dupa
        <p className='login-option' onClick={() => setMode("login")}>Cancle</p>
        </>);

    var switchMode = () => {
        let cc;
        switch (mode) {
            case "register":
                cc = reg;
                break;
            case "password":
                cc = pass;
                break;
            case "login":
            default:
                cc = log;
                break;
        }
        return cc;
    }

    useEffect(() => {
        setContent(switchMode());
        setMessage("");
    }, [mode]);

    return (<> {logged ?
        <></>
        :
        <div className='auth-window'>
            <div className='auth-window-inner'>
                {content}
                <p className="warn-message">{message}</p>
                <button className={disabled ? "disable" : ""}  onClick={() => auth(mode)} >{mode}</button>
            </div>
        </div>
        }
    </>)
}

export default AuthWindow;