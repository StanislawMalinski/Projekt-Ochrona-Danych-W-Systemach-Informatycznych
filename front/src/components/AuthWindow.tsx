import { useEffect, useState } from 'react';
import '../styles/AuthWindow.css'
import LoginComp from './auth/LoginComp';
import RegisterComp from './auth/RegisterComp';

import {login, register, passwordchangerequestcode, codesubmit, submitregistrationcode, passwordchange} from '../Client';
import { saveCredentials } from '../utils/Cipher';
import PassChangeComp from './auth/PassChangeComp';


interface AuthWindowProps {
    logged: boolean;
    setLogged: (arg: boolean) => void;
    setAccount: (arg: {accountNumber: string, balance: number, history: {accountNumber: string, recipientAccountNumber: string, recipient: string, title: string, value: number}[]}) => void;
}

function AuthWindow(props: AuthWindowProps) {
    const {logged, setLogged, setAccount} = props;
    const [mode, setMode] = useState("login");
    const [button, setButton] = useState("Login");
    const [content, setContent] = useState(<></>);

    // Login
    const [loginRequest, setLoginRequest]                           = useState({email: "", password: ""})
    // Password change
    const [codeChangePasswordRequest, setCodeChangePasswordRequest] = useState({email: ""})
    const [codeRequest, setCodeRequest]                             = useState({email: "", code: ""})
    const [passwordChangeRequest, setPasswordChangeRequest]         = useState({email: "", code: "", password: ""})
    // Register
    const [registerRequest, setRegisterRequest]                     = useState({email: "", password: ""})
    const [registerCodeRequest, setRegisterCodeRequest]             = useState({email: "", code: ""})
   

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
                    } 
                    setMessage(response.message);
                });
                break;
            case "reg-input-cred":
                register(registerRequest)
                .then((response) => {
                    if (response.success) {
                        setMode("reg-input-code");
                    } 
                    setMessage(response.message);
                });
                break;
            case "reg-input-code":
                submitregistrationcode(registerCodeRequest)
                .then((response) => {
                    if (response.success) {
                        setMode("login");
                    } 
                    setMessage(response.message);
                });
            break;
            case "pass-input-mail":
                passwordchangerequestcode(codeChangePasswordRequest)
                .then((response) => {
                    console.log(response)
                    if (response.success) {
                        setMode("pass-input-code");
                        console.log("zmieniono tryb")
                    } else {
                        setMessage(response.message);
                        console.log("nie zmieniono trybu")
                    }
                    setMessage(response.message);
                });
                break;
            case "pass-input-code":
                codesubmit(codeRequest)
                .then((response) => {
                    if (response.success) {
                        setMode("pass-input-pass");
                    } 
                    setMessage(response.message);
                });
                break;
            case "pass-input-pass":
                passwordchange(passwordChangeRequest) 
                .then((response) => {
                    if (response.success) {
                        setMode("login");
                    }
                    setMessage(response.message);
                });
                break;
        }
    }

    var reg = (<>
        <RegisterComp 
            setRegisterRequest={setRegisterRequest}
            setRegisterCodeRequest={setRegisterCodeRequest}
            setMessage={(e) => setMessage(e)}
            setDisabled={(e) => setDisabled(e)}
            mode={mode} />
        <p className='login-option' onClick={() => setMode("login")}>You have an account?</p>
        </>)
    var log = (<>
        <LoginComp setLoginRequest={setLoginRequest}/>
        <p className='login-option' onClick={() => setMode("pass-input-mail")}>Forgot password?</p>
        <p className='login-option ' onClick={() => setMode("reg-input-cred")} >Sign in</p>
        </>);
    var pass = (<>
        <PassChangeComp 
            setCodeChangePasswordRequest={setCodeChangePasswordRequest}
            setPasswordChangeRequest={setPasswordChangeRequest}
            setCodeRequest={setCodeRequest}
            setDisabled={(e) => setDisabled(e)}
            mode={mode}/>
        <p className='login-option' onClick={() => setMode("login")}>Cancle</p>
        </>);

    var switchMode = () => {
        let cc;
        switch (mode) {
            case "reg-input-cred":
            case "reg-input-code":
                cc = reg;
                break;
            case "pass-input-mail":
            case "pass-input-code":
            case "pass-input-pass":
                cc = pass;
                break;
            case "login":
            default:
                cc = log;
                break;
        }
        return cc;
    }
    var switchButton = () => {
        let cc;
        switch (mode) {
            case "reg-input-cred":
                cc = "Sign in";
                break;
            case "reg-input-code":
                cc = "Verify";
                break;
            case "pass-input-mail":
                cc = "Send code";
                break;
            case "pass-input-code":
                cc = "Verify";
                break;
            case "pass-input-pass":
                cc = "Change password";
                break;
            case "login":
            default:
                cc = "Login";
                break;
        }
        return cc;
    }

    useEffect(() => {
        setContent(switchMode());
        setButton(switchButton());
        setMessage("");
    }, [mode]);

    return (<> {logged ?
        <></>
        :
        <div className='auth-window'>
            <div className='auth-window-inner'>
                {content}
                <p className="warn-message">{message}</p>
                {(disabled && (mode === "register" || mode === "pass-input-pass"))  ? <p>{button}</p> : (<button  onClick={() => auth(mode)} >{button}</button>)}
            </div>
        </div>
        }
    </>)
}

export default AuthWindow;