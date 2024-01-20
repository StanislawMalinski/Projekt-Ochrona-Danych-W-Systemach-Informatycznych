import { useEffect, useState } from "react";

interface RegisterCompProps {
    setRegisterRequest: (data: { email: string, password: string }) => void;
    setDisableRegistery: (arg: boolean) => void;
}

function RegisterComp(props: RegisterCompProps) {
    const {setRegisterRequest, setDisableRegistery} = props;

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [repeatPassword, setRepeatPassword] = useState("");

    useEffect(() => {
        setRegisterRequest({email: email, password: password})
        console.log(password, repeatPassword)
    }, [email, password]);

    useEffect(() => {
        setDisableRegistery(password !== repeatPassword);  
    }, [repeatPassword]);

    return (
        <div>
            <input type="text" placeholder="email" onChange={(e) => setEmail(e.target.value)}/>
            <input type="password" placeholder="password" onChange={(e) => setPassword(e.target.value)}/>
            <input type="password" placeholder="repeat password" onChange={(e) => setRepeatPassword(e.target.value)}/>

        </div>
    )
}

export default RegisterComp;