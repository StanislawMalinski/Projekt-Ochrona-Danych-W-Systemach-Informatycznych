import { useState } from "react";
import PassTestInfo from "./PassTestInfo";
import PasswordInput from "./PasswordInput";


function PassComp(){
    const [pass, setPass] = useState<string>("");
    
    return(
        <div>
            <PassTestInfo pass={pass}/>
            <PasswordInput setPass={setPass}/>
        </div>
    )
}

export default PassComp;