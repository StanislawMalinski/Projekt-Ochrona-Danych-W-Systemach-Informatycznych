import { useState } from "react";

function PasswordInput() {
    const [inp, setInp] = useState(<input></input>); 
    return (<>
        <div className="password-input">
            {inp}
        </div>
    </>);
}

export default PasswordInput;