import { useEffect, useState } from "react";
import '../../styles/AuthWindow.css';

interface LoginCompProps {
    setLoginRequest: (data: { email: string, password: string }) => void
    setCodeRequest: (data: { email: string, code: string }) => void
    mode: string
}

function LoginComp(props: LoginCompProps) {
    const {setLoginRequest, setCodeRequest,mode} = props;
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [code, setCode] = useState("");
    
    useEffect(() => {
        setLoginRequest({email: email, password: password})
    }, [email, password])
    
    useEffect(() => {
        setCodeRequest({email: email, code: code})
    }, [code])

    return (
        <div className="auth-form">
            <h1>Login</h1>
            {mode == "login" ? <>
            <input className="auth-input" type="text" placeholder="email" onChange={(e) => setEmail(e.target.value)}/>
            <input className="auth-input" type="password" placeholder="password" onChange={(e) => setPassword(e.target.value)}/></>
            : 
            <input className="auth-input" type="text" placeholder="code" onChange={(e) => setCode(e.target.value)}/>}
        </div>
    )
}

export default LoginComp;