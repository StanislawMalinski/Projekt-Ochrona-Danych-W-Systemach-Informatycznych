import { useState } from "react";
import PassTestInfo from "./PassTestInfo";
import PasswordInput from "./PasswordInput";

import './styles/PasswordInput.css';

interface PassCompProps {
    setDisabled: (arg: boolean) => void;
    setPass: (arg: string) => void;
    pass: string;
}


function PassComp(props: PassCompProps){
    const {pass, setPass, setDisabled} = props;
    const [repeat, setRepeat] = useState("");

    return(
        <div>
            <div className="pass-component">
                <div className="left-pass-component">
                    <PasswordInput setPass={setPass}/>
                    <PasswordInput setPass={setRepeat}/>
                </div>
                <div className="right-pass-component">
                    <PassTestInfo pass={pass} reapeatPass={repeat} setDisabled={setDisabled} />
                </div>
            </div>
        </div>
    )
}

export default PassComp;