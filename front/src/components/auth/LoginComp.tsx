import { useEffect, useState } from "react";
import '../../styles/AuthWindow.css';

interface LoginCompProps {
    setLoginRequest: (data: { email: string, password: string }) => void;
}

function LoginComp(props: LoginCompProps) {
    const {setLoginRequest} = props;
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    
    useEffect(() => {
        setLoginRequest({email: email, password: password})
    }, [email, password])
    
    return (
        <div className="auth-form">
            <h1>Login</h1>
            <input className="auth-input" type="text" placeholder="email" onChange={(e) => setEmail(e.target.value)}/>
            <input className="auth-input" type="password" placeholder="password" onChange={(e) => setPassword(e.target.value)}/>            
        </div>
    )
}

export default LoginComp;