import { useEffect, useState } from "react";
import { passCheck, passStrenght } from "../../service/PassCheck";
import './styles/PasswordInput.css'


interface PassTestInfoProps {
    pass: string;
    reapeatPass: string;
    setDisabled: (arg: boolean) => void;
}

function PassTestInfo(props: PassTestInfoProps){
    const {pass, reapeatPass, setDisabled} = props;
    const [equal, setEqual] = useState(false);
    const [strenght, setStrenht] = useState(0);
    const [check, setCheck] = useState({'containsUpperLetters': false,
    'containsLowerLetters': false,
    'containsSpecialChars': false,
    'containsNumbers'     : false,
    'isAtLeastEightLetter': false,
    'isGoodPass'           : false});


    useEffect(() => {
        setStrenht(passStrenght(pass));
        setCheck(passCheck(pass));
        setEqual(pass === reapeatPass && pass !== "");
    }, [pass]);
    
    useEffect(() => {
        setEqual(pass === reapeatPass && pass !== "");
    }, [reapeatPass]);

    useEffect(() => {
        setDisabled(
        check.containsLowerLetters || 
        check.containsUpperLetters ||
        check.containsSpecialChars ||
        check.containsNumbers      ||
        check.isAtLeastEightLetter ||
        check.isGoodPass           ||
        equal);
    }, [pass, reapeatPass]);
    return (<>
            <table>
                <tbody>
                    <tr>
                        <td>
                            Contains Lower Case Letters
                        </td>
                        <td>
                        {check.containsLowerLetters ? <p className='good'>✅</p>: <p className="bad">❌</p> }
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contains Upper Case Letters
                        </td>
                        <td>
                        {check.containsUpperLetters ? <p className='good'>✅</p>: <p className="bad">❌</p> }
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contains Numbers
                        </td>
                        <td>
                        {check.containsNumbers ? <p className='good'>✅</p>: <p className="bad">❌</p> }
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contains Special Characters
                        </td>
                        <td>
                        {check.containsSpecialChars ? <p className='good'>✅</p>: <p className="bad">❌</p> }
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is At Least Eight Letter
                        </td>
                        <td>
                        {check.isAtLeastEightLetter ? <p className='good'>✅</p>: <p className="bad">❌</p> }
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Passwords are the same.
                        </td>
                        <td>
                        {equal ? <p className='good'>✅</p>: <p className="bad">❌</p> }
                        </td>
                    </tr>
                </tbody>
            </table>
            </>);
}

export default PassTestInfo;