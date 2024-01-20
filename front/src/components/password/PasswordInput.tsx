import './styles/PasswordInput.css'

interface PasswordInputProps {
    setPass: (arg: string) => void;
}


function PasswordInput(props: PasswordInputProps) {
    const {setPass} = props;

    return (<>    
            <input type='password' onChange={(e) => {
                setPass(e.target.value);
            }}></input>
    </>);
}

export default PasswordInput;