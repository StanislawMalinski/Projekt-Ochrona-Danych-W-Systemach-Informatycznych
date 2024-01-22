import { useEffect, useState } from "react";
import PassComp from "../password/PassComp";

interface RegisterCompProps {
    setRegisterRequest: (data: { email: string, password: string, name: string }) => void;
    setMessage: (message: string) => void;
    setDisabled: (arg: boolean) => void;
}

function RegisterComp(props: RegisterCompProps) {
    const {setRegisterRequest, setDisabled} = props;

    const [email, setEmail] = useState("");
    const [name, setName] = useState("");
    const [password, setPassword] = useState("");

    useEffect(() => {
        setRegisterRequest({email: email, password: password, name: name})
    }, [email, password, name]);



    return (
        <div className="auth-form">
            <h1>Register</h1>
            <input className="auth-input" type="text" placeholder="email" onChange={(e) => setEmail(e.target.value)}/>
            <input className="auth-input" type="text" placeholder="name" onChange={(e) => setName(e.target.value)}/>
            <PassComp setPass={(e) => setPassword(e)} pass={password} setDisabled={(e) => setDisabled(e)}/>
        </div>
    )
}

export default RegisterComp;