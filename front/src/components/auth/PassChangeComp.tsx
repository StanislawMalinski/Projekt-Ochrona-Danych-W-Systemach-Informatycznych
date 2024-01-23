import { useEffect, useState } from "react";
import PassComp from "../password/PassComp";


interface PassChangeCompProps {
    setCodeChangePasswordRequest: (arg: {email: string}) => void;
    setCodeRequest: (arg: {email: string, code: string}) => void;
    setPasswordChangeRequest: (arg: {email: string, code: string, password: string}) => void;
    setDisabled: (arg: boolean) => void;
    mode: string;
}

function PassChangeComp(props: PassChangeCompProps) {
    const {setCodeChangePasswordRequest, mode, setCodeRequest, setPasswordChangeRequest, setDisabled} = props;

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [code, setCode] = useState("");

    const [content, setContent] = useState(<></>);
    
    useEffect(() => {
        setCodeChangePasswordRequest({email: email});
        setCodeRequest({email: email, code: code});
        setPasswordChangeRequest({email: email, code: code, password: password});
    }, [email]);

    useEffect(() => {
        setCodeRequest({email: email, code: code});
        setPasswordChangeRequest({email: email, code: code, password: password});
    }, [code]);

    useEffect(() => {
        setPasswordChangeRequest({email: email, code: code, password: password});
    }, [password]);

    var contentSwitch = (m: string) => {
        switch (m) {
            case "pass-input-code":
                return (
                    <>
                    <p>The verificaion message was sent to provided email address: {email}</p>
                    <input name='code' type="text" placeholder="code" onChange={(e) => setCode(e.target.value)}/>
                    </>
                );
            case "pass-input-pass":
                return (
                    <>
                    <p>Set up your new password</p>
                    <PassComp setPass={(e) => setPassword(e)} setDisabled={(e) => setDisabled(e)} pass={password} />
                    </>
                );
            case "pass-input-mail":
            default:
                return (
                    <>
                    <p>Provide your email address</p>
                    <input name='email' type="text" placeholder="email" onChange={(e) => setEmail(e.target.value) }/>
                    </>
                );
        }
    };
    
    useEffect(() => {
        setContent(contentSwitch(mode));
    }, [mode]);

    return (
        <>
            <h1>Forgot the password?</h1>
            {content}
        </>
    )
}

export default PassChangeComp
