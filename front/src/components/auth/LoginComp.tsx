import { useEffect, useState } from "react";

interface LoginCompProps {
    setLoginRequest: (data: { email: string, password: string }) => void;
}

function LoginComp(props: LoginCompProps) {
    const {setLoginRequest} = props;
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    useEffect(() => {
        setLoginRequest({email: email, password: password})
        console.log()
    }, [email, password])
    
    return (
        <div>
            <input type="text" placeholder="email" onChange={(e) => setEmail(e.target.value)}/>
            <input type="password" placeholder="password" onChange={(e) => setPassword(e.target.value)}/>            
        </div>
    )
}

export default LoginComp;