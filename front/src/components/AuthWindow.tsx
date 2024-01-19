import { useState } from 'react';
import '../styles/AuthWindow.css'
import PasswordInput from './PasswordInput';


interface AuthWindowProps {
    logged: boolean;
    setLogged: (arg: boolean) => void;
}

function AuthWindow(props: AuthWindowProps) {
    const {logged, setLogged} = props;
    const [hasAc, setHasAc] = useState(true);

    var content = hasAc ? "Login" : "Register"
    var goTo = hasAc ? "Register" : "Login"

    return (<> {logged ?
        <></>
        :
        <div className='auth-window'>
            <div className='auth-window-inner'>
                <h1>{content}</h1>
                <PasswordInput/>
                <button onClick={() => setHasAc(!hasAc)}>{goTo}</button>
                <button onClick={() => setLogged(!logged)}>"Login"</button>
            </div>
        </div>
        }
    </>)
}

export default AuthWindow;