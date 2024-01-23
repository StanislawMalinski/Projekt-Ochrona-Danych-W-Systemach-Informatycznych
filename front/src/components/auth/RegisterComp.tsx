import { useEffect, useState } from "react";
import PassComp from "../password/PassComp";

interface RegisterCompProps {
    setRegisterRequest: (data: { email: string, password: string, name: string }) => void;
    setRegisterCodeRequest: (data: { email: string, code: string }) => void;
    setMessage: (message: string) => void;
    setDisabled: (arg: boolean) => void;
    mode: string;
}

function RegisterComp(props: RegisterCompProps) {
    const {setRegisterRequest, setDisabled, mode, setRegisterCodeRequest} = props;

    const [email, setEmail] = useState("");
    const [name, setName] = useState("");
    const [password, setPassword] = useState("");
    const [code, setCode] = useState("");

    const [content, setContent] = useState(<></>);

    useEffect(() => {
        setRegisterRequest({email: email, password: password, name: name})
    }, [password, name]);

    useEffect(() => {
        setRegisterCodeRequest({email: email, code: code})
    }, [code]);

    useEffect(() => {
        setRegisterCodeRequest({email: email, code: code})
        setRegisterRequest({email: email, password: password, name: name})
    }, [email]);

    useEffect(() => {
        setContent(switchRegister(mode));
    }, [mode]);

    const switchRegister = (m: String) => {
        switch (m) {
            case "reg-input-code":
                return (<>
                    <p>The verification code was sent to provided email address</p>
                    <input className="auth-input" type="text" placeholder="code" onChange={(e) => setCode(e.target.value)}/>
                    </>
                );
            case "reg-input-cred":
            default:
                return (<>
                    <p>Provide your credentials</p>
                    <input className="auth-input" type="text" placeholder="email" onChange={(e) => setEmail(e.target.value)}/>
                    <input className="auth-input" type="text" placeholder="name" onChange={(e) => setName(e.target.value)}/>
                    <PassComp setPass={(e) => setPassword(e)} pass={password} setDisabled={(e) => setDisabled(e)}/>
                    </>
                );
        }
    };

    return (
        <div className="auth-form">
            <h1>Register</h1>
            {content}
        </div>
    )
}

export default RegisterComp;