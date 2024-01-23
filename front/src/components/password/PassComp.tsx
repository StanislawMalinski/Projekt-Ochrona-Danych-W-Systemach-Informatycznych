import { useEffect, useState } from "react";
import PassTestInfo from "./PassTestInfo";
import PasswordInput from "./PasswordInput";

import './styles/PasswordInput.css';

interface PassCompProps {
    setDisabled: (arg: boolean) => void;
    setPass: (arg: string) => void;
    pass: string;
}


function PassComp(props: PassCompProps){
    const { setPass, setDisabled} = props;
    const [repeat, setRepeat] = useState("");
    const [internalPass, setInternalPass] = useState("");


    useEffect(() => {
        setPass(internalPass);
        console.log("pass: ", internalPass);
    }, [internalPass]);

    return(
        <div>
            <div className="pass-component">
                <div className="left-pass-component">
                    <PasswordInput setPass={setInternalPass}/>
                    <PasswordInput setPass={setRepeat}/>
                </div>
                <div className="right-pass-component">
                    <PassTestInfo pass={internalPass} reapeatPass={repeat} setDisabled={setDisabled} />
                </div>
            </div>
        </div>
    )
}

export default PassComp;